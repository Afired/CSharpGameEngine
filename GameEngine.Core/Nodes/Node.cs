using System.Collections.Generic;

namespace GameEngine.Core.Nodes; 

public class Node {
    
    public List<Node> ChildNodes { get; }
    public Node? ParentNode { get; }
    
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
    
}
