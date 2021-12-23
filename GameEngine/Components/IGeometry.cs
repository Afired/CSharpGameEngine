using GameEngine.Core;
using GameEngine.Rendering;
using OpenGL;

namespace GameEngine.Components; 

public class Geometry : Component {
    
    public uint Vao { get; private set; }
    public uint Vbo { get; private set; }
    public int VertexCount { get; }
    private float[] VertexData { get; set; }


    public Geometry(GameObject gameObject, float[] vertexData) : base(gameObject) {
        VertexData = vertexData;
        VertexCount = vertexData.Length / 5;
        RenderingEngine.OnLoad += InitializeGeometry;
    }

    private void InitializeGeometry() {
        
        Vao = GL.glGenVertexArray();
        Vbo = GL.glGenBuffer();
        
        GL.glBindVertexArray(Vao);
        GL.glBindBuffer(GL.GL_ARRAY_BUFFER, Vbo);
        

        unsafe {
            fixed(float* v = &VertexData[0]) {
                GL.glBufferData(GL.GL_ARRAY_BUFFER, sizeof(float) * VertexData.Length, v, GL.GL_STATIC_DRAW);
            }
            
            // xyz
            GL.glVertexAttribPointer(0, 3, GL.GL_FLOAT, false, 5 * sizeof(float), (void*) (0 * sizeof(float)));
            GL.glEnableVertexAttribArray(0);
            
            // texture coordinates
            GL.glVertexAttribPointer(1, 2, GL.GL_FLOAT, false, 5 * sizeof(float), (void*) (3 * sizeof(float)));
            GL.glEnableVertexAttribArray(1);

            GL.glBindBuffer(GL.GL_ARRAY_BUFFER, 0);
            GL.glBindVertexArray(0);
        }
        
    }
    
}

public interface IGeometry {
    public Geometry Geometry { get; set; }
}
