using System.Collections.Generic;
using GameEngine.Core.Serialization;

namespace GameEngine.Core.Nodes;

public partial class TestNode : Node {
    
    [Serialized] private float[] MyFloatArray { get; set; } = new[] {
        1f, 2f, 3f
    };
    
//    [Serialized] private List<float> MyFloatList { get; set; } = new() {
//        1f,
//        2f,
//        3f,
//    };
    
    [Serialized] private List<string> Names { get; set; }
    
}
