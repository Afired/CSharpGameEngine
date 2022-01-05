using GameEngine.Entities;
using GameEngine.Numerics;
using GameEngine.Rendering;
using GameEngine.Rendering.Shaders;
using Silk.NET.OpenGL;

namespace GameEngine.Components; 

public class Renderer : Component {

    public string Texture { get; set; }
    public string Shader { get; set; }
    
    
    public Renderer(Entity entity, string texture, string shader) : base(entity) {
        RenderingEngine.OnLoad += OnLoad;
        Texture = texture;
        Shader = shader;
    }

    private void OnLoad() {
        LayerStack.DefaultNormalLayer.OnDraw += OnDraw;
    }
    
    public void OnDraw() {
        ShaderRegister.Get(Shader).Use();

        Transform transform = (Entity as ITransform).Transform;
        
        Matrix4x4 trans = Matrix4x4.CreateTranslation(transform.Position.X, transform.Position.Y, transform.Position.Z);
        Matrix4x4 sca = Matrix4x4.CreateScale(transform.Scale.X, transform.Scale.Y, transform.Scale.Z);
        Matrix4x4 rotMat = Matrix4x4.CreateRotationZ(transform.Rotation);
        
        ShaderRegister.Get(Shader).SetMatrix4x4("model", sca * rotMat * trans);
        ShaderRegister.Get(Shader).SetMatrix4x4("projection", RenderingEngine.CurrentCamera.GetProjectionMatrix());
        
        Gl.BindVertexArray((Entity as IGeometry).Geometry.Vao);
        
        TextureRegister.Get(Texture).Bind();
        ShaderRegister.Get(Shader).SetInt("u_Texture", 0);

        Gl.DrawArrays(PrimitiveType.Triangles, 0, (uint) (Entity as IGeometry).Geometry.VertexCount);
        Gl.BindVertexArray(0);
    }
    
}

public interface IRenderer : ITransform, IGeometry {
    Renderer Renderer { get; set; }
}
