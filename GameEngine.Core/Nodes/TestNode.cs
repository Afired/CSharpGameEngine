using System;
using System.Collections.Generic;
using System.Linq;
using GameEngine.Core.Serialization;

namespace GameEngine.Core.Nodes;

public partial class TestNode : Node {
    
    [Serialized] private float[] MyFloatArray { get; set; } = new[] {
        1f, 2f, 3f
    };
    
//    [Serialized] public Many<string>? MyStringList { get; set; } = new Many<string>() {
//        genericObjects = new[] {
//            "1",
//            "2",
//            "3",
//        }
//    };

}

public class Many<T1> : IMany {

    public T1[] genericObjects;

    public object[] objects => genericObjects.Cast<object>().ToArray();
}

public interface IMany {
    
    public object[] objects { get; }

}