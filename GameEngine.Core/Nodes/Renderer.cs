using GameEngine.Core.Numerics;
using GameEngine.Core.Rendering;
using GameEngine.Core.Rendering.Geometry;
using GameEngine.Core.Rendering.Shaders;
using Silk.NET.OpenGL;

namespace GameEngine.Core.Nodes; 

public partial class Renderer : Transform {

    public string Texture { get; set; } = "checkerboard";
    public string Shader { get; set; } = "default";
    public string Geometry { get; set; } = "quad";
    
    
    protected override void OnDraw() {
        ShaderRegister.Get(Shader).Use();
        
        Matrix4x4 trans = Matrix4x4.CreateTranslation(Position.X, Position.Y, Position.Z);
        Matrix4x4 sca = Matrix4x4.CreateScale(Scale.X, Scale.Y, Scale.Z);
        Matrix4x4 rotMat = Matrix4x4.CreateRotationZ(Rotation);
        
        ShaderRegister.Get(Shader).SetMatrix4x4("model", sca * rotMat * trans);
        ShaderRegister.Get(Shader).SetMatrix4x4("projection", RenderingEngine.CurrentCamera.GetProjectionMatrix());
        
        Geometry geometry = GeometryRegister.Get(Geometry);
        
        Gl.BindVertexArray(geometry.Vao);
        
        TextureRegister.Get(Texture).Bind();
        ShaderRegister.Get(Shader).SetInt("u_Texture", 0);
        
        Gl.DrawArrays(PrimitiveType.Triangles, 0, (uint) geometry.VertexCount);
        Gl.BindVertexArray(0);
    }
    
}
