using GameEngine.Core.AssetManagement;
using GameEngine.Core.Rendering.Geometry;
using GameEngine.Core.Rendering.Shaders;
using GameEngine.Core.Rendering.Textures;
using GameEngine.Core.Serialization;
using GlmNet;
using Silk.NET.OpenGL;
using Shader = GameEngine.Core.Rendering.Shaders.Shader;

namespace GameEngine.Core.Nodes; 

public partial class SpriteRenderer : Transform3D {
    
    [Serialized] public AssetRef<Texture2D> Texture { get; set; }
    [Serialized] public AssetRef<Shader> Shader { get; set; }
    
    // new indexed drawing
    protected override unsafe void OnDraw() {
        
        Mesh mesh = AssetDatabase.Get<Mesh>(Mesh.QuadGuid) ?? (Mesh) Mesh.Default;
        
        Shader.Get().Use();
        
//        mat4 transformMat2D = glm.translate(new mat4(1), new vec3(WorldPosition.X, WorldPosition.Y, WorldPosition.Z)) *
//                            glm.rotate(WorldRotation, new vec3(0, 0, 1)) *
//                            glm.scale(new mat4(1), new vec3(WorldScale.X, WorldScale.Y, WorldScale.Z));
        
        mat4 transformMat = glm.translate(new mat4(1), new vec3(WorldPosition.X, WorldPosition.Y, WorldPosition.Z)) *
                            glm.rotate(WorldRotation.X / 10, new vec3(1, 0, 0)) *
                            glm.rotate(WorldRotation.Y / 10, new vec3(0, 1, 0)) *
                            glm.rotate(WorldRotation.Z / 10, new vec3(0, 0, 1)) *
                            glm.scale(new mat4(1), new vec3(WorldScale.X, WorldScale.Y, WorldScale.Z));
        
        Shader.Get().GLM_SetMat("model", transformMat);
        Shader.Get().GLM_SetMat("projection", Rendering.Renderer.CurrentCamera.GLM_GetProjectionMatrix());
        
        Texture.Get().Bind();
        Shader.Get().SetInt("u_Texture", 0);
        
        Gl.BindVertexArray(mesh.Vao);
        Gl.DrawElements(PrimitiveType.Triangles, (uint) mesh.EboLength, DrawElementsType.UnsignedInt, null); // can't use indices here, just pass in nullptr and it will use last bound
        Gl.BindVertexArray(0);
    }
    
//    protected override void OnDraw() {
//        
//        Mesh? mesh = AssetDatabase.Get<Mesh>(Mesh.QuadGuid);
//        
//        Shader.Get().Use();
//        
////        Matrix4x4 trans = Matrix4x4.CreateTranslation(Position.X, Position.Y, Position.Z);
////        Matrix4x4 sca = Matrix4x4.CreateScale(Scale.X, Scale.Y, Scale.Z);
////        Matrix4x4 rotMat = Matrix4x4.CreateRotationZ(Rotation);
//
//        mat4 transformMat = glm.translate(new mat4(1), new vec3(Position.X, Position.Y, Position.Z)) *
//                            glm.rotate(Rotation, new vec3(0, 0, 1)) *
//                            glm.scale(new mat4(1), new vec3(Scale.X, Scale.Y, Scale.Z));
//        
//        
////        ShaderRegister.Get(Shader).SetMatrix4x4("model", sca * rotMat * trans);
////        ShaderRegister.Get(Shader).SetMatrix4x4("projection", RenderingEngine.CurrentCamera.GetProjectionMatrix());
//        Shader.Get().GLM_SetMat("model", transformMat);
//        Shader.Get().GLM_SetMat("projection", Rendering.Renderer.CurrentCamera.GLM_GetProjectionMatrix());
//        
//        Gl.BindVertexArray(mesh.Vao);
//        
//        Texture.Get().Bind();
//        Shader.Get().SetInt("u_Texture", 0);
//        
//        Gl.DrawArrays(PrimitiveType.Triangles, 0, (uint) mesh.VertexCount);
//        Gl.BindVertexArray(0);
//    }
    
}
