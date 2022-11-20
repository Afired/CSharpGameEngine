using System;

namespace GameEngine.Core.Nodes;

//public interface Has<in T> where T : Node { }

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class Has<T> : Attribute where T : Node {
//    public Has(string name) { }
//    public Has(Expression<Func<T, object>> selector) { }
}
