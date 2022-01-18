using GameEngine.AutoGenerator;
using GameEngine.Entities;
using GameEngine.Numerics;
using GameEngine.Rendering;
using GameEngine.Rendering.Shaders;
using Silk.NET.OpenGL;

namespace GameEngine.Components; 

[RequireComponent(typeof(ITransform), typeof(IGeometry))]
public partial class Renderer : Component {

    public string Texture { get; set; }
    public string Shader { get; set; }
    
    
    protected override void Init() {
        RenderingEngine.OnLoad += OnLoad;
    }
    
    private void OnLoad() {
        LayerStack.DefaultNormalLayer.OnDraw += OnDraw;
    }
    
    public void OnDraw() {
        ShaderRegister.Get(Shader).Use();

        Transform transform = Transform;
        
        Matrix4x4 trans = Matrix4x4.CreateTranslation(transform.Position.X, transform.Position.Y, transform.Position.Z);
        Matrix4x4 sca = Matrix4x4.CreateScale(transform.Scale.X, transform.Scale.Y, transform.Scale.Z);
        Matrix4x4 rotMat = Matrix4x4.CreateRotationZ(transform.Rotation);
        
        ShaderRegister.Get(Shader).SetMatrix4x4("model", sca * rotMat * trans);
        ShaderRegister.Get(Shader).SetMatrix4x4("projection", RenderingEngine.CurrentCamera.GetProjectionMatrix());
        
        Gl.BindVertexArray(Geometry.Vao);
        
        TextureRegister.Get(Texture).Bind();
        ShaderRegister.Get(Shader).SetInt("u_Texture", 0);

        Gl.DrawArrays(PrimitiveType.Triangles, 0, (uint) Geometry.VertexCount);
        Gl.BindVertexArray(0);
    }
    
}
