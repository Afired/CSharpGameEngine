using GameEngine.Core.Numerics;
using GameEngine.Core.Rendering;
using GameEngine.Core.Rendering.Geometry;
using GameEngine.Core.Rendering.Shaders;
using GameEngine.Core.Serialization;
using GlmNet;
using Silk.NET.OpenGL;

namespace GameEngine.Core.Nodes; 

public partial class Renderer : Transform {
    
    [Serialized] public string Texture { get; set; } = "checkerboard";
    [Serialized] public string Shader { get; set; } = "default";
    [Serialized] public string Geometry { get; set; } = "quad";
    
    protected override void OnDraw() {
        ShaderRegister.Get(Shader).Use();
        
//        Matrix4x4 trans = Matrix4x4.CreateTranslation(Position.X, Position.Y, Position.Z);
//        Matrix4x4 sca = Matrix4x4.CreateScale(Scale.X, Scale.Y, Scale.Z);
//        Matrix4x4 rotMat = Matrix4x4.CreateRotationZ(Rotation);

        mat4 transformMat = glm.translate(new mat4(1), new vec3(Position.X, Position.Y, Position.Z)) *
                            glm.rotate(Rotation, new vec3(0, 0, 1)) *
                            glm.scale(new mat4(1), new vec3(Scale.X, Scale.Y, Scale.Z));
        
        
//        ShaderRegister.Get(Shader).SetMatrix4x4("model", sca * rotMat * trans);
//        ShaderRegister.Get(Shader).SetMatrix4x4("projection", RenderingEngine.CurrentCamera.GetProjectionMatrix());
        ShaderRegister.Get(Shader).GLM_SetMat("model", transformMat);
        ShaderRegister.Get(Shader).GLM_SetMat("projection", Rendering.Renderer.CurrentCamera.GLM_GetProjectionMatrix());
        
        Geometry geometry = GeometryRegister.Get(Geometry);
        
        Gl.BindVertexArray(geometry.Vao);
        
        TextureRegister.Get(Texture).Bind();
        ShaderRegister.Get(Shader).SetInt("u_Texture", 0);
        
        Gl.DrawArrays(PrimitiveType.Triangles, 0, (uint) geometry.VertexCount);
        Gl.BindVertexArray(0);
    }
    
}
