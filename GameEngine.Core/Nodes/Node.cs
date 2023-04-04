using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using GameEngine.Core.AssetManagement;
using GameEngine.Core.Serialization;
using JetBrains.Annotations;

namespace GameEngine.Core.Nodes; 

[UsedImplicitly(ImplicitUseTargetFlags.WithInheritors)]
public class Node : IAsset {
    
    public IReadOnlyList<Node> ChildNodes => _childNodes;
    [Serialized(Editor.Hidden)] public Node? ParentNode { get; internal set; }
    [Serialized(Editor.Hidden)] private readonly List<Node> _childNodes = null!;
    internal bool HasBeenAwoken { get; private set; } = false;
    protected virtual bool AwakeThisBeforeItsChildren => false;
    
    public Node GetRootNode() {
        Node currentNode = this;
        Node? nextNode = currentNode.ParentNode;
        while(nextNode is not null) {
            currentNode = nextNode;
            nextNode = currentNode.ParentNode;
        }
        return currentNode;
    }
    
    internal void Awake() {
        if(!AwakeThisBeforeItsChildren)
            foreach(Node childNodes in ChildNodes)
                childNodes.Awake();
        if(!HasBeenAwoken) {
#if TRY_CATCH
            try {
#endif
                OnAwake();
#if TRY_CATCH
            } catch(Exception e) {
                Console.LogError($"{this.GetType()}: {e}");
            }
#endif
            HasBeenAwoken = true;
        }
        if(AwakeThisBeforeItsChildren)
            foreach(Node childNodes in ChildNodes)
                childNodes.Awake();
    }
    
    internal void Update() {
#if TRY_CATCH
        try {
#endif
            OnUpdate();
#if TRY_CATCH
        } catch(Exception e) {
            Console.LogError($"{this.GetType()}: {e}");
        }
#endif
        foreach(Node childNode in ChildNodes)
            childNode.Update();
    }
    
    internal void PrePhysicsUpdate() {
#if TRY_CATCH
        try {
#endif
            OnPrePhysicsUpdate();
#if TRY_CATCH
        } catch(Exception e) {
            Console.LogError($"{this.GetType()}: {e}");
        }
#endif
        foreach(Node childNode in ChildNodes) {
            childNode.PrePhysicsUpdate();
        }
    }
    
    internal void PhysicsUpdate() {
#if TRY_CATCH
        try {
#endif
            OnPhysicsUpdate();
#if TRY_CATCH
        } catch(Exception e) {
            Console.LogError($"{this.GetType()}: {e}");
        }
#endif
        foreach(Node childNode in ChildNodes)
            childNode.PhysicsUpdate();
    }
    
    internal void Draw() {
#if TRY_CATCH
        try {
#endif
            OnDraw();
#if TRY_CATCH
        } catch(Exception e) {
            Console.LogError($"{this.GetType()}: {e}");
        }
#endif
        foreach(Node childNode in ChildNodes)
            childNode.Draw();
    }
    
    protected virtual void OnAwake() { }
    protected virtual void OnUpdate() { }
    protected virtual void OnPrePhysicsUpdate() { }
    protected virtual void OnPhysicsUpdate() { }
    protected virtual void OnDraw() { }
    
    public static T New<T>() where T : Node {
        return (T) New(typeof(T));
    }
    
    public static Node New(Type type) {
        if(!type.IsAssignableTo(typeof(Node)))
            throw new Exception();
        
        Node parentNode = (Node) Activator.CreateInstance(type)!;
        List<Node> childNodes = new();
        typeof(Node).GetField(nameof(_childNodes), BindingFlags.NonPublic | BindingFlags.Instance)!.SetValue(parentNode, childNodes);
        
        foreach(PropertyInfo childNodeInfo in type.GetProperties(BindingFlags.Public | BindingFlags.Instance).Where(p => p.PropertyType.IsAssignableTo(typeof(Node)) && p.Name != nameof(ParentNode))) {
            Node childNode = New(childNodeInfo.PropertyType);
            childNodeInfo.SetValue(parentNode, childNode);
            childNodes.Add(childNode);
            typeof(Node).GetProperty(nameof(ParentNode), BindingFlags.Public | BindingFlags.Instance)!.SetValue(childNode, parentNode);
        }
        
        foreach(PropertyInfo nodeArrInfo in type.GetProperties(BindingFlags.Public | BindingFlags.Instance).Where(p => p.PropertyType.IsAssignableTo(typeof(INodeArr)))) {
            INodeArr nodeArr = (INodeArr) Activator.CreateInstance(nodeArrInfo.PropertyType)!;
            nodeArrInfo.SetValue(parentNode, nodeArr);
            typeof(INodeArr).GetProperty("ContainingNode", BindingFlags.NonPublic | BindingFlags.Instance)!.SetValue(nodeArr, parentNode);
        }
        
        return parentNode;
    }
    
}
