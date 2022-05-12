using GameEngine.Core.Serialization;

namespace GameEngine.Core.Rendering; 

public struct Color {
    
    [Serialized] public float R { get; internal set; }
    [Serialized] public float G { get; internal set; }
    [Serialized] public float B { get; internal set; }
    [Serialized] public float A { get; internal set; }
    
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
