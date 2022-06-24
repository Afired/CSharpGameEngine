using System.Collections.Generic;
using System.Runtime.Serialization;
using GameEngine.Core.Serialization;
using Newtonsoft.Json;

namespace GameEngine.Core.Nodes; 

public class Node {
    
    public IReadOnlyList<Node> ChildNodes => _childNodes;
    
//? https://www.newtonsoft.com/json/help/html/PreserveObjectReferences.htm#:~:text=References%20cannot%20be,work%20with%20PreserveReferencesHandling.
    public Node? ParentNode { get; internal set; }
    [Serialized(Editor.Hidden)] protected List<Node> _childNodes { private get; init; } = null!;
    private bool _hasBeenAwaken = false;
    
    protected Node(Node? parentNode, out List<Node> childNodes) {
        childNodes = new List<Node>();
        ParentNode = parentNode;
    }
    
    public Node(Node? parentNode) {
        _childNodes = new List<Node>();
        ParentNode = parentNode;
    }
    
    [JsonConstructor]
    protected Node(bool isJsonConstructed) { }
    
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
    
    [OnDeserialized]
    private void OnAfterDeserialization(StreamingContext context) {
        foreach(Node childNode in ChildNodes)
            childNode.ParentNode = this;
    }
    
}
