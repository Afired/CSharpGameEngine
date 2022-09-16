using System.Numerics;
using Silk.NET.OpenGL;

namespace GameEngine.Core.Rendering.Geometry; 

public class PosMesh : Mesh {

    public PosMesh(_Vertex[] vertexData) : base() {
        VertexCount = vertexData.Length;
        InitializeGeometry(vertexData);
    }
    
    private unsafe void InitializeGeometry(_Vertex[] vertexData) {
        
        Vao = Gl.GenVertexArray();
        Vbo = Gl.GenBuffer();
        
        Gl.BindVertexArray(Vao);
        Gl.BindBuffer(BufferTargetARB.ArrayBuffer, Vbo);
        
        fixed(void* v = &vertexData[0]) {
            Gl.BufferData(BufferTargetARB.ArrayBuffer, (nuint) (sizeof(Vector3) * VertexCount), v, BufferUsageARB.StaticDraw);
        }
        
        // position:xyz
        //                     index in shader, how many elements, element type, should be normalized, size, offset from start
        Gl.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), (void*) (0 * sizeof(float)));
        Gl.EnableVertexAttribArray(0);
        
        Gl.BindBuffer(BufferTargetARB.ArrayBuffer, 0);
        Gl.BindVertexArray(0);
    }
    
}
