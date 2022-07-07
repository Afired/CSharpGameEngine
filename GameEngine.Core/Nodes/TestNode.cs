using System;
using System.Collections.Generic;
using GameEngine.Core.Serialization;

namespace GameEngine.Core.Nodes;

// before we directly compared the actual strings which means this is not being detected: [GameEngine.AutoGenerator.GenerateComponentInterface]
// now we directly look if the attribute string contains the name
//todo: even this would work because we check for the actual string containing the name: [Something.Blablabla.GenerateComponentInterface.Blabla]

public partial class TestNode : Node/*, Has<Transform>, Arr<Transform>*/ {
    
    // [Serialized] public NodeArr<Node> MyNodeArray { get; set; }
    // [Serialized] public List<Node> MyNodeList { get; set; }
    [Serialized] public GenericClass<BaseClass> MyIntGenericClass { get; set; }

    protected override void OnAwake() {
        // MyNodeArray = new NodeArr<Node>(this);
        // MyNodeList = new List<Node>();
        MyIntGenericClass = new GenericClass<BaseClass>();
        MyIntGenericClass.Values.Add(new BaseClass());
        MyIntGenericClass.Values.Add(new Class1());
    }
    
}

public class GenericClass<T> {
    [Serialized] public List<T> Values { get; set; } = new();
}

public class BaseClass { }
public class Class1 : BaseClass { }
public class Class2 : BaseClass { }
