using GameEngine.Core.Ecs;
using GameEngine.Core.Numerics;
using GameEngine.Core.Rendering;
using GameEngine.Core.Rendering.Geometry;
using GameEngine.Core.Rendering.Shaders;
using Silk.NET.OpenGL;

namespace GameEngine.Core.Components; 

public partial class Renderer : Node {

    public string Texture { get; set; } = "checkerboard";
    public string Shader { get; set; } = "default";
    public string Geometry { get; set; } = "quad";
    
    
    protected override void OnDraw() {
        ShaderRegister.Get(Shader).Use();
        
        Transform transform = Transform;
        
        Matrix4x4 trans = Matrix4x4.CreateTranslation(transform.Position.X, transform.Position.Y, transform.Position.Z);
        Matrix4x4 sca = Matrix4x4.CreateScale(transform.Scale.X, transform.Scale.Y, transform.Scale.Z);
        Matrix4x4 rotMat = Matrix4x4.CreateRotationZ(transform.Rotation);
        
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
