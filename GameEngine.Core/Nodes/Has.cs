using System;
using System.Collections.Generic;
using GameEngine.Core.Serialization;

namespace GameEngine.Core.Nodes;

//public class Node {
//    public bool Has<T>() where T : Node {
//        return this is Has<T>;
//    }
//}


public partial class TestNode2 : Node { }
public partial class TestNode3 : Node { }
public partial class TestNode4 : Node { }


#region Has<T> Arr<T>

public interface Has<in T> where T : Node { }

public interface Arr<in T> where T : Node? { }


public partial class MyTestNode1 : Node, Has<TestNode2>, Arr<TestNode3?>/*, Has<Test>*/ {
    
}

// auto generated
// public partial class MyTestNode1 {
//     
//     [GameEngine.Core.Serialization.Serialized(GameEngine.Core.Serialization.Editor.Hidden)] public GameEngine.Core.Nodes.TestNode2 TestNode2 { get; init; } = null!;
//     [GameEngine.Core.Serialization.Serialized(GameEngine.Core.Serialization.Editor.Hidden)] public GameEngine.Core.Nodes.TestNode3 TestNode3 { get; init; } = null!;
//
//     public MyTestNode1(GameEngine.Core.Nodes.Node? parentNode = null) : this(parentNode, out System.Collections.Generic.List<GameEngine.Core.Nodes.Node> childNodes) {
//         _childNodes = childNodes;
//     }
//     
//     public MyTestNode1(GameEngine.Core.Nodes.Node? parentNode, out System.Collections.Generic.List<GameEngine.Core.Nodes.Node> childNodes) : base(parentNode, out childNodes) {
//         TestNode2 = new GameEngine.Core.Nodes.TestNode2(this);
//         TestNode3 = new GameEngine.Core.Nodes.TestNode3(this);
//
//         childNodes.Add(TestNode2);
//         childNodes.Add(TestNode3);
//     }
//     
//     [Newtonsoft.Json.JsonConstructor]
//     protected MyTestNode1(bool isJsonConstructed) : base(isJsonConstructed) { }
//     
// }

#endregion
