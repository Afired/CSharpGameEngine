using System;
using System.Collections.Generic;
using GameEngine.Core.Serialization;

namespace GameEngine.Core.Nodes; 

[Serializable]
public class Node {
    
    [Serialized] public List<Node> ChildNodes { get; set; }
    [Serialized] public Node? ParentNode { get; /*private*/ set; }
    
    protected Node(Node? parentNode) {
        ChildNodes = new List<Node>();
        ParentNode = parentNode;
    }
    
    public Node GetRootNode() {
        Node currentNode = this;
        Node? nextNode = currentNode.ParentNode;
        while(nextNode is not null) {
            currentNode = nextNode;
            nextNode = currentNode.ParentNode;
        }
        return currentNode;
    }
    
    internal void Awake(Node? parentNode = null) {
        ParentNode = parentNode;
        OnAwake();
        foreach(Node childNodes in ChildNodes)
            childNodes.Awake(this);
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
    
}
