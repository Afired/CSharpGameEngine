using System;
using GameEngine.Core.AssetManagement;
using GameEngine.Core.Numerics;
using GameEngine.Core.Rendering.Geometry;
using GameEngine.Core.Serialization;
using GlmNet;
using Silk.NET.OpenGL;
using Shader = GameEngine.Core.Rendering.Shaders.Shader;
using Texture = GameEngine.Core.Rendering.Textures.Texture;

namespace GameEngine.Core.Nodes; 

public partial class MeshRenderer : Transform {
    
    [Serialized] public AssetRef<Texture> Texture { get; set; }
    [Serialized] public AssetRef<Shader> Shader { get; set; }
    [Serialized] public AssetRef<Mesh> Mesh { get; set; }
    [Serialized] public AssetRef<Model> Model { get; set; }
    
    [Serialized] public Vector3 Rotation3D { get; private set; }
    
//    protected override void OnDraw() {
//        Shader.Get().Use();
//        
//        mat4 transformMat = glm.translate(new mat4(1), new vec3(Position.X, Position.Y, Position.Z)) *
//                            glm.rotate(Rotation3D.X / 10, new vec3(1, 0, 0)) *
//                            glm.rotate(Rotation3D.Y / 10, new vec3(0, 1, 0)) *
//                            glm.rotate(Rotation3D.Z / 10, new vec3(0, 0, 1)) *
//                            glm.scale(new mat4(1), new vec3(Scale.X, Scale.Y, Scale.Z));
//        
//        Shader.Get().GLM_SetMat("model", transformMat);
//        Shader.Get().GLM_SetMat("projection", Rendering.Renderer.CurrentCamera.GLM_GetProjectionMatrix());
//        Texture.Get().Bind();
//        Shader.Get().SetInt("u_Texture", 0);
//        Shader.Get().SetFloat("time", Time.TotalTimeElapsed);
//        
//        Mesh? geometry = Mesh.Get();
//        if(geometry is null)
//            return;
//        
//        Gl.BindVertexArray(geometry.Vao);
//        
//        if(geometry is PosUvNormalMeshIndexedBuffer posUvNormalGeometryEbo) {
//            unsafe {
//                // indexed drawing - currently doesnt work :/
//                Gl.DrawElements(PrimitiveType.Triangles, (uint) posUvNormalGeometryEbo.EboLength, DrawElementsType.UnsignedInt, null);
//            }
//        } else {
//            // normal drawing
//            Gl.DrawArrays(PrimitiveType.Triangles, 0, (uint) geometry.VertexCount);
//        }
//        
//        Gl.BindVertexArray(0);
//    }
    
    protected override void OnDraw() {
        Shader.Get().Use();
        
        mat4 transformMat = glm.translate(new mat4(1), new vec3(Position.X, Position.Y, Position.Z)) *
                            glm.rotate(Rotation3D.X / 10, new vec3(1, 0, 0)) *
                            glm.rotate(Rotation3D.Y / 10, new vec3(0, 1, 0)) *
                            glm.rotate(Rotation3D.Z / 10, new vec3(0, 0, 1)) *
                            glm.scale(new mat4(1), new vec3(Scale.X, Scale.Y, Scale.Z));
        
        Shader.Get().GLM_SetMat("model", transformMat);
        Shader.Get().GLM_SetMat("projection", Rendering.Renderer.CurrentCamera.GLM_GetProjectionMatrix());
        Texture.Get().Bind();
        Shader.Get().SetInt("u_Texture", 0);
        Shader.Get().SetFloat("time", Time.TotalTimeElapsed);

        Model? model = Model.Get();
        
        if(model is null)
            return;

        Mesh[] meshes = model.Meshes;

        for(int i = 0; i < meshes.Length; i++) {
            
            Gl.BindVertexArray(meshes[i].Vao);
        
            if(meshes[i] is PosUvNormalMeshIndexedBuffer posUvNormalGeometryEbo) {
                unsafe {
                    // indexed drawing - currently doesnt work :/
                    Gl.DrawElements(PrimitiveType.Triangles, (uint) posUvNormalGeometryEbo.EboLength, DrawElementsType.UnsignedInt, null);
                }
            } else {
                // normal drawing
                Gl.DrawArrays(PrimitiveType.Triangles, 0, (uint) meshes[i].VertexCount);
            }
            
            Gl.BindVertexArray(0);
            
        }
        
    }
    
}
