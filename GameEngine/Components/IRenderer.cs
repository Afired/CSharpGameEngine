using GameEngine.Core;
using GameEngine.Numerics;
using GameEngine.Rendering;
using GameEngine.Rendering.Shaders;
using GLFW;
using OpenGL;

namespace GameEngine.Components; 

public class Renderer : Component {

    private string _texture;
    private string _shader;
    
    
    public Renderer(GameObject gameObject, string texture, string shader) : base(gameObject) {
        RenderingEngine.OnDraw += OnDraw;
        _texture = texture;
        _shader = shader;
    }
    
    public void OnDraw() {
        ShaderRegister.Get(_shader).Use();

        Transform transform = (GameObject as ITransform).Transform;
        
        Matrix4x4 trans = Matrix4x4.CreateTranslation(transform.Position.X, transform.Position.Y, transform.Position.Z);
        Matrix4x4 sca = Matrix4x4.CreateScale(transform.Scale.X, transform.Scale.Y, transform.Scale.Z);
        Matrix4x4 rotMat = Matrix4x4.CreateRotationZ(transform.Rotation);
        
        ShaderRegister.Get(_shader).SetMatrix4x4("model", sca * rotMat * trans);
        ShaderRegister.Get(_shader).SetMatrix4x4("projection", RenderingEngine.CurrentCamera.GetProjectionMatrix());
        
        GL.glBindVertexArray((GameObject as IGeometry).Geometry.Vao);
        
        TextureRegister.Get(_texture).Bind();
        ShaderRegister.Get(_shader).SetInt("u_Texture", 0);

        GL.glDrawArrays(GL.GL_TRIANGLES, 0, (GameObject as IGeometry).Geometry.VertexCount);
        GL.glBindVertexArray(0);
    }
    
}

public interface IRenderer : ITransform, IGeometry {
    Renderer Renderer { get; set; }
}
