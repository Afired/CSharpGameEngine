using System;
using GameEngine.Core.AssetManagement;
using Silk.NET.OpenGL;

namespace GameEngine.Core.Rendering.Geometry; 

public class Mesh : IAsset {
    
    public uint Vao { get; private set; }
    public uint Vbo { get; private set; }
    public int VertexCount { get; private set; }
    public uint Ebo { get; private set; }
    public int EboLength { get; private set; }
    
    public static readonly Guid QuadGuid = new("605b3a35-5e06-4cc4-8da2-3f2d07471b51");
    
    public static Mesh Quad { get; }
    
    static Mesh() {
        Quad = CreateQuad(Application.Instance.Renderer);
    }
    
    internal static Mesh CreateQuad(Renderer renderer) {
        Vertex[] quadVertexData = {
            new(new(-0.5f, 0.5f, 0.0f), new(0.0f, 1.0f), new()),
            new(new(0.5f, 0.5f, 0.0f), new(1.0f, 1.0f), new()),
            new(new(-0.5f, -0.5f, 0.0f), new(0.0f, 0.0f), new()),
            new(new(0.5f, -0.5f, 0.0f), new(1.0f, 0.0f), new()),
        };
        uint[] indexData = {
            0, 1, 2,
            1, 2, 3,
        };
        return new Mesh(quadVertexData, indexData, renderer);
    }
    
    public Mesh(Vertex[] vertexData, uint[] indices, Renderer renderer) {
        VertexCount = vertexData.Length;
        InitializeGeometry(vertexData, indices, renderer);
        EboLength = indices.Length;
    }
    
    private unsafe void InitializeGeometry(Vertex[] vertexData, uint[] indexData, Renderer renderer) {
        
        Vao = renderer.MainWindow.Gl.GenVertexArray();
        Vbo = renderer.MainWindow.Gl.GenBuffer();
        Ebo = renderer.MainWindow.Gl.GenBuffer();
        
        renderer.MainWindow.Gl.BindVertexArray(Vao);
        
        // vbo
        renderer.MainWindow.Gl.BindBuffer(BufferTargetARB.ArrayBuffer, Vbo);
        fixed(Vertex* vertexDataPtr = &vertexData[0]) {
            renderer.MainWindow.Gl.BufferData(BufferTargetARB.ArrayBuffer, (nuint) (sizeof(Vertex) * vertexData.Length), vertexDataPtr, BufferUsageARB.StaticDraw);
        }
        
        // ebo
        renderer.MainWindow.Gl.BindBuffer(BufferTargetARB.ElementArrayBuffer, Ebo);
        fixed(uint* indexDataPtr = &indexData[0]) {
            renderer.MainWindow.Gl.BufferData(BufferTargetARB.ElementArrayBuffer, (nuint) (sizeof(uint) * indexData.Length), indexDataPtr, BufferUsageARB.StaticDraw);
        }
        
        // position:xyz
        //                     index in shader, how many elements, element type, should be normalized, size, offset from start
        renderer.MainWindow.Gl.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 8 * sizeof(float), (void*) (0 * sizeof(float)));
        renderer.MainWindow.Gl.EnableVertexAttribArray(0);
        
        // texture:uv
        renderer.MainWindow.Gl.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, 8 * sizeof(float), (void*) (3 * sizeof(float)));
        renderer.MainWindow.Gl.EnableVertexAttribArray(1);
        
        // normal:xyz
        renderer.MainWindow.Gl.VertexAttribPointer(2, 3, VertexAttribPointerType.Float, true, 8 * sizeof(float), (void*) (5 * sizeof(float)));
        renderer.MainWindow.Gl.EnableVertexAttribArray(2);
        
        
        // note that this is allowed, the call to glVertexAttribPointer registered VBO as the vertex attribute's bound vertex buffer object so afterwards we can safely unbind
        renderer.MainWindow.Gl.BindBuffer(BufferTargetARB.ArrayBuffer, 0);
        
        // remember: do NOT unbind the EBO while a VAO is active as the bound element buffer object IS stored in the VAO; keep the EBO bound.
        //glBindBuffer(GL_ELEMENT_ARRAY_BUFFER, 0);
        // You can unbind the VAO afterwards so other VAO calls won't accidentally modify this VAO, but this rarely happens. Modifying other
        // VAOs requires a call to glBindVertexArray anyways so we generally don't unbind VAOs (nor VBOs) when it's not directly necessary.
        renderer.MainWindow.Gl.BindVertexArray(0);
    }
    
}
