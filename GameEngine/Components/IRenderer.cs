using GameEngine.Core;
using GameEngine.Numerics;
using GameEngine.Rendering.Shaders;
using GLFW;
using OpenGL;

namespace GameEngine.Components; 

public class Renderer : Component {
    
    public Renderer(GameObject gameObject) : base(gameObject) {
        Game.OnDraw += OnDraw;
    }
    
    public void OnDraw() {
        ShaderRegister.Get("default").Use();

        Transform transform = (GameObject as ITransform).Transform;
        
        Matrix4x4 trans = Matrix4x4.CreateTranslation(transform.Position.X, transform.Position.Y, transform.Position.Z);
        Matrix4x4 sca = Matrix4x4.CreateScale(transform.Scale.X, transform.Scale.Y, transform.Scale.Z);
        Matrix4x4 rotMat = Matrix4x4.CreateRotationZ(transform.Rotation);
        
        ShaderRegister.Get("default").SetMatrix4x4("model", sca * rotMat * trans);
        ShaderRegister.Get("default").SetMatrix4x4("projection", Game.CurrentCamera.GetProjectionMatrix());
        
        GL.glBindVertexArray((GameObject as IGeometry).Geometry.Vao);
        
        TextureRegister.Get("Checkerboard").Bind();
        ShaderRegister.Get("default").SetInt("u_Texture", 0);

        GL.glDrawArrays(GL.GL_TRIANGLES, 0, (GameObject as IGeometry).Geometry.VertexCount);
        GL.glBindVertexArray(0);
    }
    
}

public interface IRenderer : ITransform, IGeometry {
    Renderer Renderer { get; set; }
}
