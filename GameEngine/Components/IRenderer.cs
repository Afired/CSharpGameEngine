using GameEngine.Core;
using GameEngine.Numerics;
using GameEngine.Rendering.Shaders;
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
        Matrix4x4 rot = Matrix4x4.CreateFromQuaternion(transform.Rotation);
        
        ShaderRegister.Get("default").SetMatrix4x4("model", rot * sca * trans);
        ShaderRegister.Get("default").SetMatrix4x4("projection", Game.CurrentBaseCamera.GetProjectionMatrix());
        
        GL.glBindVertexArray((GameObject as IGeometry).Geometry.Vao);
        GL.glDrawArrays(GL.GL_TRIANGLES, 0, 18);
        GL.glBindVertexArray(0);
    }
    
}

public interface IRenderer : ITransform, IGeometry {
    Renderer Renderer { get; set; }
}
