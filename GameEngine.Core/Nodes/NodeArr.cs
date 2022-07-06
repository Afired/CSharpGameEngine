using System;
using System.Collections.Generic;
using GameEngine.Core.Serialization;

namespace GameEngine.Core.Nodes; 

//? cant implement IEnumerable because deserialization is failing -> cant Convert NodeArr<Node> into ICollection<Node>
//TODO: delay adding and removing Nodes until beginning of next game tick
public sealed class NodeArr<T> : INodeArr/*, IEnumerable<T>*/ where T : Node {
    
    [Serialized(Editor.Hidden)] private List<T> _list = new();
    [Serialized(Editor.Hidden)] Node INodeArr.ContainingNode { get; set; } = null!;
    
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
        (((INodeArr) this).ContainingNode.ChildNodes as List<Node>)!.Add(node);
        node.ParentNode = ((INodeArr) this).ContainingNode;
    }
    
    public void Remove(T node) {
        if(!_list.Contains(node))
            throw new Exception("Cant remove node that is not contained in that node arr");
        
        _list.Remove(node);
        (((INodeArr) this).ContainingNode.ChildNodes as List<Node>)!.Remove(node);
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
    internal Node ContainingNode { get; set; }
}
