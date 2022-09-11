using System;
using Silk.NET.OpenGL;

namespace GameEngine.Core.Rendering.Geometry; 

public class Geometry {
    
    public uint Vao { get; protected set; }
    public uint Vbo { get; protected set; }
    public int VertexCount { get; protected set; }
    public static readonly Guid QuadGuid = new("605b3a35-5e06-4cc4-8da2-3f2d07471b51");
    
    public Geometry(float[] vertexData) {
        VertexCount = vertexData.Length / 5;
        InitializeGeometry(vertexData);
    }
    
    protected Geometry() { }
    
    private void InitializeGeometry(float[] vertexData) {
        
        Vao = Gl.GenVertexArray();
        Vbo = Gl.GenBuffer();
        
        Gl.BindVertexArray(Vao);
        Gl.BindBuffer(BufferTargetARB.ArrayBuffer, Vbo);
        

        unsafe {
            fixed(float* v = &vertexData[0]) {
                Gl.BufferData(BufferTargetARB.ArrayBuffer, (nuint) (sizeof(float) * vertexData.Length), v, BufferUsageARB.StaticDraw);
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
