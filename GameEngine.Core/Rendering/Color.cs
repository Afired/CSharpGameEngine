using GameEngine.Core.Serialization;

namespace GameEngine.Core.Rendering; 

public struct Color {
    
    [Serialized] public float R { get; set; }
    [Serialized] public float G { get; set; }
    [Serialized] public float B { get; set; }
    [Serialized] public float A { get; set; }
    
    public Color(float r, float g, float b) {
        R = r;
        G = g;
        B = b;
        A = 1.0f;
    }
    
    public Color(float r, float g, float b, float a) {
        R = r;
        G = g;
        B = b;
        A = a;
    }
    
}
