using Silk.NET.OpenGL;

namespace GameEngine.Core.Rendering.Geometry; 

public class PosUvNormalMeshIndexedBuffer : Mesh {
    
    public uint Ebo { get; protected set; }
    public int EboLength { get; private set; }

    public PosUvNormalMeshIndexedBuffer(_Vertex[] vertexData, uint[] indices) : base() {
        VertexCount = vertexData.Length;
        InitializeGeometry(vertexData, indices);
        EboLength = indices.Length;
    }
    
    private unsafe void InitializeGeometry(_Vertex[] vertexData, uint[] indexData) {
        
        Vao = Gl.GenVertexArray();
        Vbo = Gl.GenBuffer();
        Ebo = Gl.GenBuffer();
        
        Gl.BindVertexArray(Vao);
        
        // vbo
        Gl.BindBuffer(BufferTargetARB.ArrayBuffer, Vbo);
        fixed(_Vertex* vertexDataPtr = &vertexData[0]) {
            Gl.BufferData(BufferTargetARB.ArrayBuffer, (nuint) (sizeof(_Vertex) * vertexData.Length), vertexDataPtr, BufferUsageARB.StaticDraw);
        }
        
        // ebo
        Gl.BindBuffer(BufferTargetARB.ElementArrayBuffer, Ebo);
        fixed(uint* indexDataPtr = &indexData[0]) {
            Gl.BufferData(BufferTargetARB.ElementArrayBuffer, (nuint) (sizeof(uint) * indexData.Length), indexDataPtr, BufferUsageARB.StaticDraw);
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
        
        
        // note that this is allowed, the call to glVertexAttribPointer registered VBO as the vertex attribute's bound vertex buffer object so afterwards we can safely unbind
        Gl.BindBuffer(BufferTargetARB.ArrayBuffer, 0);
        
        // remember: do NOT unbind the EBO while a VAO is active as the bound element buffer object IS stored in the VAO; keep the EBO bound.
        //glBindBuffer(GL_ELEMENT_ARRAY_BUFFER, 0);
        // You can unbind the VAO afterwards so other VAO calls won't accidentally modify this VAO, but this rarely happens. Modifying other
        // VAOs requires a call to glBindVertexArray anyways so we generally don't unbind VAOs (nor VBOs) when it's not directly necessary.
        Gl.BindVertexArray(0);
        
    }
    
}
