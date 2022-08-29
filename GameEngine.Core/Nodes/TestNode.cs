using System.Collections.Generic;
using GameEngine.Core.Numerics;
using GameEngine.Core.Serialization;

namespace GameEngine.Core.Nodes;

public partial class TestNode : Node {
    
//    [Serialized] private float[] MyFloatArray { get; set; } = new[] {
//        1f, 2f, 3f
//    };
    
//    [Serialized] private List<float> MyFloatList { get; set; } = new() {
//        1f,
//        2f,
//        3f,
//    };
    
//    [Serialized] private List<List<string>> _nestedStringList;

//    [Serialized] private string[,] _nestedStringArray = new string[3, 3];
    
    [Serialized] private Dictionary<string, float> SerializedDictionary { get; set; } = new() {
        { "First", 1f },
        { "SomeOtherKey", 420f },
        { "Nice", 69f },
    };
    
//    [Serialized] private string TestNullString { get; set; }

//    [Serialized] private List<float> FloatList { get; set; }

}
