using System;
using GameEngine.Core.AssetManagement;
using GameEngine.Core.Rendering.Geometry;
using GameEngine.Core.Rendering.Shaders;
using GameEngine.Core.Rendering.Textures;
using GameEngine.Core.Serialization;
using GlmNet;
using Silk.NET.OpenGL;
using Shader = GameEngine.Core.Rendering.Shaders.Shader;
using Texture = GameEngine.Core.Rendering.Textures.Texture;

namespace GameEngine.Core.Nodes; 

public partial class SpriteRenderer : Transform {
    
    [Serialized] public Guid Texture { get; set; }
    [Serialized] public Guid Shader { get; set; }
    
    protected override void OnDraw() {
        AssetDatabase.Get<Shader>(Shader).Use();
        
//        Matrix4x4 trans = Matrix4x4.CreateTranslation(Position.X, Position.Y, Position.Z);
//        Matrix4x4 sca = Matrix4x4.CreateScale(Scale.X, Scale.Y, Scale.Z);
//        Matrix4x4 rotMat = Matrix4x4.CreateRotationZ(Rotation);

        mat4 transformMat = glm.translate(new mat4(1), new vec3(Position.X, Position.Y, Position.Z)) *
                            glm.rotate(Rotation, new vec3(0, 0, 1)) *
                            glm.scale(new mat4(1), new vec3(Scale.X, Scale.Y, Scale.Z));
        
        
//        ShaderRegister.Get(Shader).SetMatrix4x4("model", sca * rotMat * trans);
//        ShaderRegister.Get(Shader).SetMatrix4x4("projection", RenderingEngine.CurrentCamera.GetProjectionMatrix());
        AssetDatabase.Get<Shader>(Shader).GLM_SetMat("model", transformMat);
        AssetDatabase.Get<Shader>(Shader).GLM_SetMat("projection", Rendering.Renderer.CurrentCamera.GLM_GetProjectionMatrix());
        
        Geometry? geometry = AssetDatabase.Get<Geometry>(Geometry.QuadGuid);
        if(geometry is null)
            return;
        
        Gl.BindVertexArray(geometry.Vao);
        
        AssetDatabase.Get<Texture>(Texture).Bind();
        AssetDatabase.Get<Shader>(Shader).SetInt("u_Texture", 0);
        
        Gl.DrawArrays(PrimitiveType.Triangles, 0, (uint) geometry.VertexCount);
        Gl.BindVertexArray(0);
    }
    
}
