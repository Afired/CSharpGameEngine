using System.Linq;
using ObjLoader.Loader.Data.VertexData;
using Silk.NET.OpenGL;

namespace GameEngine.Core.Rendering.Geometry; 

public class PosUvNormalGeometryIndexedBuffer : Geometry {
    
    public uint Ebo { get; protected set; }
    public int EboLength { get; private set; }

    public PosUvNormalGeometryIndexedBuffer(_Vertex[] vertexData, uint[] indices) : base() {
        VertexCount = vertexData.Length;
        InitializeGeometry(vertexData, indices);
        EboLength = indices.Length;
    }
    
    private void InitializeGeometry(_Vertex[] vertexData, uint[] indices) {
        
        Vao = Gl.GenVertexArray();
        Vbo = Gl.GenBuffer();
        Ebo = Gl.GenBuffer();
        
        Gl.BindVertexArray(Vao);
        Gl.BindBuffer(BufferTargetARB.ArrayBuffer, Vbo);
        unsafe {
            fixed(_Vertex* v = &vertexData[0]) {
                Gl.BufferData(BufferTargetARB.ArrayBuffer, (nuint) (sizeof(_Vertex) * vertexData.Length), v, BufferUsageARB.StaticDraw);
            }
            
            
            
            Gl.BindBuffer(BufferTargetARB.ElementArrayBuffer, Ebo);
            unsafe {
                fixed(uint* v = &indices[0]) {
                    Gl.BufferData(BufferTargetARB.ElementArrayBuffer, (nuint) (sizeof(uint) * indices.Length), v, BufferUsageARB.StaticDraw);
                }
            }
            
            
            
            // position:xyz
            //                     index in shader, how many elements, element type, should be normalized, size, offset from start
            Gl.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 8 * sizeof(float), (void*) (0 * sizeof(float)));
            Gl.EnableVertexAttribArray(0);
            
            // texture:uv
            Gl.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, 8 * sizeof(float), (void*) (3 * sizeof(float)));
            Gl.EnableVertexAttribArray(1);
            
            // normal:xyz
            Gl.VertexAttribPointer(2, 3, VertexAttribPointerType.Float, true, 8 * sizeof(float), (void*) (5 * sizeof(float)));
            Gl.EnableVertexAttribArray(2);
            
            /*
             * A VAO stores the glBindBuffer calls when the target is GL_ELEMENT_ARRAY_BUFFER.
             * This also means it stores its unbind calls so make sure you don't unbind the element array buffer before unbinding your VAO,
             * otherwise it doesn't have an EBO configured.
             */
            // note that this is allowed, the call to glVertexAttribPointer registered VBO as the vertex attribute's bound vertex buffer object so afterwards we can safely unbind
            Gl.BindBuffer(BufferTargetARB.ArrayBuffer, 0);
            Gl.BindVertexArray(0);
        }
        
    }
    
}
