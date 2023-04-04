using System;

namespace GameEngine.Core.Nodes;

//public interface Has<in T> where T : Node { }

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class Has<TNode> : Attribute where TNode : Node, new() {
//    public Has(string name) { }
//    public Has(Expression<Func<T, object>> selector) { }
}

//[Has<Camera3D>]
//[Has<ICamera, Camera3D>]
//[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
//public class Has<TNode, TDefaultNode> : Attribute where TNode : INode where TDefaultNode : TNode, new() {
//    
//}
//
//public interface INode {
//    
//}