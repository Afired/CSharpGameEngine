namespace GameEngine.Geometry; 

public class Geometry {
    
    public float[] Vertices { get; set; }


    public Geometry() {
        Vertices = new[] {
            -0.5f, 0.5f, 1f, 0f, 0f,  // top left
            0.5f, 0.5f, 0f, 1f, 0f,   // top right
            -0.5f, -0.5f, 0f, 0f, 1f, // bottom left

            0.5f, 0.5f, 0f, 1f, 0f,   // top right
            0.5f, -0.5f, 0f, 1f, 1f,  // bottom right
            -0.5f, -0.5f, 0f, 0f, 1f, // bottom left
        };
    }
    
}
