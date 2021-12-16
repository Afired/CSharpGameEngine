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

        ITransform transform = GameObject as ITransform;
        
        Matrix4x4 trans = Matrix4x4.CreateTranslation(transform.Transform.Position.X, transform.Transform.Position.Y, transform.Transform.Position.Z);
        Matrix4x4 sca = Matrix4x4.CreateScale(transform.Transform.Scale.X, transform.Transform.Scale.Y, transform.Transform.Scale.Z);
        Matrix4x4 rot = Matrix4x4.CreateFromQuaternion(transform.Transform.Rotation);
        
        ShaderRegister.Get("default").SetMatrix4x4("model", rot * sca * trans);
        ShaderRegister.Get("default").SetMatrix4x4("projection", Game.CurrentBaseCamera.GetProjectionMatrix());
        
        GL.glBindVertexArray((GameObject as IGeometry).Geometry.Vao);
        GL.glDrawArrays(GL.GL_TRIANGLES, 0, 18);
        GL.glBindVertexArray(0);
    }
    
}

public interface IRenderer {
    Renderer Renderer { get; set; }
}
