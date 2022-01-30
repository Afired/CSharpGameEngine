using Silk.NET.OpenGL;

namespace GameEngine.Rendering.Geometry; 

public class Geometry {
    
    public uint Vao { get; private set; }
    public uint Vbo { get; private set; }
    public int VertexCount { get; private set; }

    private float[] _vertexData;

    public Geometry(float[] vertexData) {
        _vertexData = vertexData;
        VertexCount = vertexData.Length / 5;
        InitializeGeometry();
    }
    
    private void InitializeGeometry() {
        
        Vao = Gl.GenVertexArray();
        Vbo = Gl.GenBuffer();
        
        Gl.BindVertexArray(Vao);
        Gl.BindBuffer(BufferTargetARB.ArrayBuffer, Vbo);
        

        unsafe {
            fixed(float* v = &_vertexData[0]) {
                Gl.BufferData(BufferTargetARB.ArrayBuffer, (nuint) (sizeof(float) * _vertexData.Length), v, BufferUsageARB.StaticDraw);
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
