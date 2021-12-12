namespace GameEngine; 

public class Scale {
    
    public float X { get; set; }
    public float Y { get; set; }
    public float Z { get; set; }


    public Scale() {
        X = 1;
        Y = 1;
        Z = 1;
    }
    
    public Scale(float x, float y, float z) {
        X = x;
        Y = y;
        Z = z;
    }
    
}
