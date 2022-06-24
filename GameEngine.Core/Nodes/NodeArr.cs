using System;
using System.Collections;
using System.Collections.Generic;
using GameEngine.Core.Serialization;
using Newtonsoft.Json;

namespace GameEngine.Core.Nodes; 

//? cant implement IEnumerable because deserialization is failing -> cant Convert NodeArr<Node> into ICollection<Node>
public sealed class NodeArr<T> : INodeArr/*, IEnumerable<T>*/ where T : Node {
    
    [Serialized(Editor.Hidden)] private List<T> _list = new();
    [Serialized(Editor.Hidden)] private Node _containingNode;
    
    public NodeArr(Node containingNode) {
        _containingNode = containingNode;
    }
    
    [JsonConstructor]
    [Obsolete]
    public NodeArr() { }
    
    IEnumerator<Node> INodeArr.GetEnumerator() {
        return GetEnumerator();
    }
    
    public IEnumerator<T> GetEnumerator() {
        return _list.GetEnumerator();
    }
    
    // IEnumerator IEnumerable.GetEnumerator() {
    //     return GetEnumerator();
    // }
    
    public void Add(T node) {
        if(node.ParentNode is not null)
            throw new Exception("Cant add node that already has a parent node");
        
        _list.Add(node);
        (_containingNode.ChildNodes as List<Node>)!.Add(node);
        node.ParentNode = _containingNode;
    }
    
    public void Remove(T node) {
        if(!_list.Contains(node))
            throw new Exception("Cant remove node that is not contained in that node arr");
        
        _list.Remove(node);
        (_containingNode.ChildNodes as List<Node>)!.Remove(node);
    }
    
    void INodeArr.Add(Node node) {
        if(node is T t)
            Add(t);
        else
            throw new ArgumentException($"Node has to be assignable to {typeof(T)}");
    }
    
    void INodeArr.Remove(Node node) {
        if(node is T t)
            Remove(t);
        else
            throw new ArgumentException($"Node has to be assignable to {typeof(T)}");
    }
    
    Type INodeArr.GetNodeType => typeof(T);
    
    public int Count => _list.Count;
    
}

public interface INodeArr {
    public void Add(Node node);
    public void Remove(Node node);
    public IEnumerator<Node> GetEnumerator();
    public Type GetNodeType { get; }
    public int Count { get; }
}