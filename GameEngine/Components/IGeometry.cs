using GameEngine.Core;
using OpenGL;

namespace GameEngine.Components; 

public class Geometry : Component {

    public uint Vao { get; private set; }
    public uint Vbo { get; private set; }
    private float[] Vertices { get; set; }
    
    
    public Geometry(GameObject gameObject, float[] vertices) : base(gameObject) {
        Vertices = vertices;
        Game.OnLoad += InitializeGeometry;
    }

    private void InitializeGeometry() {
        
        Vao = GL.glGenVertexArray();
        Vbo = GL.glGenBuffer();
        
        GL.glBindVertexArray(Vao);
        GL.glBindBuffer(GL.GL_ARRAY_BUFFER, Vbo);
        

        unsafe {
            fixed(float* v = &Vertices[0]) {
                GL.glBufferData(GL.GL_ARRAY_BUFFER, sizeof(float) * Vertices.Length, v, GL.GL_STATIC_DRAW);
            }
            
            //xyz
            GL.glVertexAttribPointer(0, 3, GL.GL_FLOAT, false, 3 * sizeof(float), (void*) (0 * sizeof(float)));
            GL.glEnableVertexAttribArray(0);

            GL.glBindBuffer(GL.GL_ARRAY_BUFFER, 0);
            GL.glBindVertexArray(0);
        }
        
    }
    
}

public interface IGeometry {
    public Geometry Geometry { get; set; }
}
