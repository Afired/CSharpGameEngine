using System;

namespace GameEngine.Core.Nodes; 

//public interface Arr<in T> where T : Node? { }

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class Arr<T> : Attribute where T : Node? {
//    public Arr(string name) { }
//    public Arr(Expression<Func<T, object>> selector) { }
}
