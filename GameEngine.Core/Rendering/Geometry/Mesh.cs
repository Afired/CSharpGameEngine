using System;
using GameEngine.Core.AssetManagement;

namespace GameEngine.Core.Rendering.Geometry; 

public abstract class Mesh : IAsset {
    
    public uint Vao { get; protected set; }
    public uint Vbo { get; protected set; }
    public int VertexCount { get; protected set; }
    public static readonly Guid QuadGuid = new("605b3a35-5e06-4cc4-8da2-3f2d07471b51");
    
    internal static PosUvNormalMesh CreateDefault() {
        _Vertex[] quadVertexData = {
            new(new(-0.5f, 0.5f, 0.0f), new(0.0f, 1.0f), new()),
            new(new(0.5f, 0.5f, 0.0f), new(1.0f, 1.0f), new()),
            new(new(-0.5f, -0.5f, 0.0f), new(0.0f, 0.0f), new()),
            
            new(new(0.5f, 0.5f, 0.0f), new(1.0f, 1.0f), new()),
            new(new(0.5f, -0.5f, 0.0f), new(1.0f, 0.0f), new()),
            new(new(-0.5f, -0.5f, 0.0f), new(0.0f, 0.0f), new()),
        };
        return new PosUvNormalMesh(quadVertexData);
    }
    
    public void Dispose() {
        //TODO: Dispose
    }
    
}
