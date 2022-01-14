using GameEngine.AutoGenerator;
using GameEngine.Entities;
using GameEngine.Rendering;
using Silk.NET.OpenGL;

namespace GameEngine.Components; 

public class Geometry : Component {
    
    public uint Vao { get; private set; }
    public uint Vbo { get; private set; }
    public int VertexCount { get; private set; }

    private float[] _vertexData;
    public float[] VertexData {
        get => _vertexData;
        set {
            _vertexData = value;
            VertexCount = value.Length / 5;
        }
    }


    public Geometry(Entity entity) : base(entity) {
        RenderingEngine.OnLoad += InitializeGeometry;
    }

    private void InitializeGeometry() {
        
        Vao = Gl.GenVertexArray();
        Vbo = Gl.GenBuffer();
        
        Gl.BindVertexArray(Vao);
        Gl.BindBuffer(BufferTargetARB.ArrayBuffer, Vbo);
        

        unsafe {
            fixed(float* v = &VertexData[0]) {
                Gl.BufferData(BufferTargetARB.ArrayBuffer, (nuint) (sizeof(float) * VertexData.Length), v, BufferUsageARB.StaticDraw);
            }
            
            // xyz
            Gl.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 5 * sizeof(float), (void*) (0 * sizeof(float)));
            Gl.EnableVertexAttribArray(0);
            
            // texture coordinates
            Gl.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, 5 * sizeof(float), (void*) (3 * sizeof(float)));
            Gl.EnableVertexAttribArray(1);

            Gl.BindBuffer(BufferTargetARB.ArrayBuffer, 0);
            Gl.BindVertexArray(0);
        }
        
    }
    
}
