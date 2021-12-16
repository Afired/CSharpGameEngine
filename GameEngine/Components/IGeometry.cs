using GameEngine.Core;
using OpenGL;

namespace GameEngine.Components; 

public class Geometry : Component {

    public uint Vao { get; private set; }
    public uint Vbo { get; private set; }
    private float[] Vertices { get; set; }
    
    
    public Geometry(GameObject gameObject) : base(gameObject) {
        Vertices = new[] {
            -0.5f, 0.5f, 1f, 0f, 0f,  // top left
            0.5f, 0.5f, 0f, 1f, 0f,   // top right
            -0.5f, -0.5f, 0f, 0f, 1f, // bottom left

            0.5f, 0.5f, 0f, 1f, 0f,   // top right
            0.5f, -0.5f, 0f, 1f, 1f,  // bottom right
            -0.5f, -0.5f, 0f, 0f, 1f, // bottom left
        };
        Game.OnLoad += InitializeGeometry;
    }

    private void InitializeGeometry() {
        
        Vao = GL.glGenVertexArray();
        Vbo = GL.glGenBuffer();
        
        GL.glBindVertexArray(Vao);
        GL.glBindBuffer(GL.GL_ARRAY_BUFFER, Vbo);
        
        float[] vertices = {
            //walls
            0, 1, 0, 0.5f, 0.5f, 1.5f,   // top
            1, -1, 1, 0.5f, 0.5f, 1.5f,  // bottom right
            -1, -1, 1, 0.5f, 0.5f, 1.5f, // bottom left
            
            0, 1, 0, 0.5f, 1.5f, 0.5f,   // top
            -1, -1, 1, 0.5f, 1.5f, 0.5f,  // bottom right
            -1, -1, -1, 0.5f, 1.5f, 0.5f, // bottom left
            
            0, 1, 0, 1.5f, 0.5f, 0.5f,    // top
            -1, -1, -1, 1.5f, 0.5f, 0.5f, // bottom right
            1, -1, -1, 1.5f, 0.5f, 0.5f,  // bottom left
            
            0, 1, 0, 0.5f, 0.5f, 0.5f,   // top
            1, -1, -1, 0.5f, 0.5f, 0.5f, // bottom right
            1, -1, 1, 0.5f, 0.5f, 0.5f,  // bottom left
            //base
            -1, -1, -1, 0.5f, 0.5f, 0.5f,
            1, -1, -1, 0.5f, 0.5f, 0.5f,
            -1, -1, 1, 0.5f, 0.5f, 0.5f,
            
            1, -1, 1, 0.5f, 0.5f, 0.5f,
            1, -1, -1, 0.5f, 0.5f, 0.5f,
            -1, -1, 1, 0.5f, 0.5f, 0.5f,
        };

        unsafe {
            fixed(float* v = &vertices[0]) {
                GL.glBufferData(GL.GL_ARRAY_BUFFER, sizeof(float) * vertices.Length, v, GL.GL_STATIC_DRAW);
            }
            
            //xyz
            GL.glVertexAttribPointer(0, 3, GL.GL_FLOAT, false, 6 * sizeof(float), (void*) (0 * sizeof(float)));
            GL.glEnableVertexAttribArray(0);
            
            //rgb
            GL.glVertexAttribPointer(1, 3, GL.GL_FLOAT, false, 6 * sizeof(float), (void*) (3 * sizeof(float)));
            GL.glEnableVertexAttribArray(1);
            
            GL.glBindBuffer(GL.GL_ARRAY_BUFFER, 0);
            GL.glBindVertexArray(0);
        }
        
    }
    
}

public interface IGeometry {
    public Geometry Geometry { get; set; }
}
