using GameEngine.Core.AssetManagement;
using GameEngine.Core.Rendering.Geometry;
using GameEngine.Core.Rendering.Textures;
using GameEngine.Core.Serialization;
using Silk.NET.OpenGL;
using Shader = GameEngine.Core.Rendering.Shaders.Shader;

namespace GameEngine.Core.Nodes; 

public partial class SpriteRenderer : Transform3D {
    
    [Serialized] public AssetRef<Texture2D> Texture { get; set; }
    [Serialized] public AssetRef<Shader> Shader { get; set; }
    
    // new indexed drawing
    protected override unsafe void OnDraw() {
        
        Mesh mesh = AssetDatabase.Get<Mesh>(Mesh.QuadGuid) ?? Mesh.Quad;
        
        Shader.Get().Use();
        
        Shader.Get().SetMat4x4("model", LocalToWorldMatrix);
        Shader.Get().SetMat4x4("view", Application.Instance.Renderer.CurrentCamera.ViewMatrix);
        Shader.Get().SetMat4x4("projection", Application.Instance.Renderer.CurrentCamera.ProjectionMatrix);
        
        Texture.Get().Bind();
        Shader.Get().SetInt("u_Texture", 0);
        
        Application.Instance.Renderer.MainWindow.Gl.BindVertexArray(mesh.Vao);
        Application.Instance.Renderer.MainWindow.Gl.DrawElements(PrimitiveType.Triangles, (uint) mesh.EboLength, DrawElementsType.UnsignedInt, null); // can't use indices here, just pass in nullptr and it will use last bound
        Application.Instance.Renderer.MainWindow.Gl.BindVertexArray(0);
    }
    
}
