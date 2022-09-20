using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using GameEngine.Core.Serialization;
using JetBrains.Annotations;

namespace GameEngine.Core.Nodes; 

[UsedImplicitly(ImplicitUseTargetFlags.WithInheritors)]
public class Node {
    
    public IReadOnlyList<Node> ChildNodes => _childNodes;
    [Serialized(Editor.Hidden)] public Node? ParentNode { get; internal set; }
    [Serialized(Editor.Hidden)] private readonly List<Node> _childNodes = null!;
    internal bool HasBeenAwoken { get; private set; } = false;
    protected virtual bool AwakeThisNodeBeforeItsChildren => false;
    
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
        if(!AwakeThisNodeBeforeItsChildren)
            foreach(Node childNodes in ChildNodes)
                childNodes.Awake();
        if(!HasBeenAwoken) {
            try {
                OnAwake();
            } catch(Exception e) {
                Console.LogError($"{this.GetType()}: {e}");
            }
            HasBeenAwoken = true;
        }
        if(AwakeThisNodeBeforeItsChildren)
            foreach(Node childNodes in ChildNodes)
                childNodes.Awake();
    }
    
    internal void Update() {
        try {
            OnUpdate();
        } catch(Exception e) {
            Console.LogError($"{this.GetType()}: {e}");
        }
        foreach(Node childNode in ChildNodes)
            childNode.Update();
    }
    
    internal void PrePhysicsUpdate() {
        try {
            OnPrePhysicsUpdate();
        } catch(Exception e) {
            Console.LogError($"{this.GetType()}: {e}");
        }
        foreach(Node childNode in ChildNodes) {
            childNode.PrePhysicsUpdate();
        }
    }
    
    internal void PhysicsUpdate() {
        try {
            OnPhysicsUpdate();
        } catch(Exception e) {
            Console.LogError($"{this.GetType()}: {e}");
        }
        foreach(Node childNode in ChildNodes)
            childNode.PhysicsUpdate();
    }
    
    internal void Draw() {
        try {
            OnDraw();
        } catch(Exception e) {
            Console.LogError($"{this.GetType()}: {e}");
        }
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
