using GameEngine.Entities;
using GameEngine.Rendering;
using Silk.NET.OpenGL;

namespace GameEngine.Components; 

public class Geometry : Component {
    
    public uint Vao { get; private set; }
    public uint Vbo { get; private set; }
    public int VertexCount { get; }
    private float[] VertexData { get; set; }
    private GL GL => RenderingEngine.Gl;


    public Geometry(Entity entity, float[] vertexData) : base(entity) {
        VertexData = vertexData;
        VertexCount = vertexData.Length / 5;
        RenderingEngine.OnLoad += InitializeGeometry;
    }

    private void InitializeGeometry() {
        
        Vao = GL.GenVertexArray();
        Vbo = GL.GenBuffer();
        
        GL.BindVertexArray(Vao);
        GL.BindBuffer(BufferTargetARB.ArrayBuffer, Vbo);
        

        unsafe {
            fixed(float* v = &VertexData[0]) {
                GL.BufferData(BufferTargetARB.ArrayBuffer, (nuint) (sizeof(float) * VertexData.Length), v, BufferUsageARB.StaticDraw);
            }
            
            // xyz
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 5 * sizeof(float), (void*) (0 * sizeof(float)));
            GL.EnableVertexAttribArray(0);
            
            // texture coordinates
            GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, 5 * sizeof(float), (void*) (3 * sizeof(float)));
            GL.EnableVertexAttribArray(1);

            GL.BindBuffer(BufferTargetARB.ArrayBuffer, 0);
            GL.BindVertexArray(0);
        }
        
    }
    
}

public interface IGeometry {
    public Geometry Geometry { get; set; }
}
