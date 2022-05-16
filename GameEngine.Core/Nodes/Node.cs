using System.Collections.Generic;
using System.Runtime.Serialization;
using GameEngine.Core.Serialization;

namespace GameEngine.Core.Nodes; 

public class Node {
    
    //todo: refactor to array
    // [Serialized(Editor.Hidden)] public List<Node> ChildNodes { get; private set; }
    [Serialized(Editor.Hidden)] public Node[] ChildNodes { get; protected set; }
    public Node? ParentNode { get; private set; }
    
    protected Node(out List<Node> childNodes, Node? parentNode) {
        childNodes = new List<Node>();
        ChildNodes = childNodes.ToArray();
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
    
    internal void Awake() {
        OnAwake();
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
    
    [OnDeserialized]
    private void OnAfterDeserialization(StreamingContext context) {
        foreach(Node childNode in ChildNodes)
            childNode.ParentNode = this;
    }
    
}
