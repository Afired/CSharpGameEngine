namespace GameEngine.Rendering; 

public struct Color {
    
    public float R { get; set; }
    public float G { get; set; }
    public float B { get; set; }
    public float A { get; set; }
    
    
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
