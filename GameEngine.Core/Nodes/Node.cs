using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using GameEngine.Core.SceneManagement;
using GameEngine.Core.Serialization;
using Newtonsoft.Json;

namespace GameEngine.Core.Nodes; 

public class Node {
    
    public IReadOnlyList<Node> ChildNodes => _childNodes;
    
//? https://www.newtonsoft.com/json/help/html/PreserveObjectReferences.htm#:~:text=References%20cannot%20be,work%20with%20PreserveReferencesHandling.
    [Serialized(Editor.Hidden)] public Node? ParentNode { get; internal set; }
    [Serialized(Editor.Hidden)] private readonly List<Node> _childNodes = null!;
    private bool _hasBeenAwaken = false;
    
    // protected Node(Node? parentNode, out List<Node> childNodes) {
    //     childNodes = new List<Node>();
    //     ParentNode = parentNode;
    // }
    //
    // public Node(Node? parentNode) {
    //     _childNodes = new List<Node>();
    //     ParentNode = parentNode;
    // }
    //
    // [JsonConstructor]
    // protected Node(bool isJsonConstructed) { }
    // private Node(int int1) { }
    
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
        if(!_hasBeenAwaken)
            OnAwake();
        _hasBeenAwaken = true;
        foreach(Node childNodes in ChildNodes)
            childNodes.Awake();
    }
    
    internal void Update() {
        OnUpdate();
        foreach(Node childNode in ChildNodes)
            childNode.Update();
    }
    
    internal void PhysicsUpdate() {
        OnPhysicsUpdate();
        foreach(Node childNode in ChildNodes)
            childNode.PhysicsUpdate();
    }
    
    internal void Draw() {
        OnDraw();
        foreach(Node childNode in ChildNodes)
            childNode.Draw();
    }
    
    protected virtual void OnAwake() { }
    protected virtual void OnUpdate() { }
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
