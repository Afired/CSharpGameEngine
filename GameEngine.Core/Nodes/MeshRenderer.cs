using System.Collections.Generic;
using GameEngine.Core.AssetManagement;
using GameEngine.Core.Numerics;
using GameEngine.Core.Rendering;
using GameEngine.Core.Rendering.Geometry;
using GameEngine.Core.Rendering.Textures;
using GameEngine.Core.Serialization;
using GlmSharp;
using Silk.NET.OpenGL;
using glm = GlmNet.glm;
using mat4 = GlmNet.mat4;
using Shader = GameEngine.Core.Rendering.Shaders.Shader;
using vec3 = GlmNet.vec3;

namespace GameEngine.Core.Nodes; 

public partial class MeshRenderer : Transform3D {
    
    [Serialized] public AssetRef<Texture2D> Texture { get; set; }
    [Serialized] public AssetRef<Shader> Shader { get; set; }
    [Serialized] public AssetRef<Model> Model { get; set; }
//    [Serialized] public Vector3 Rotation3D { get; private set; }
    
    [Serialized] public List<AssetRef<Texture2D>>? Textures { get; set; } = new();
    
    [Serialized] public Vector3 DirectionalLightRotation { get; set; } = Vector3.Down;
    [Serialized] public Color DirectionalLightColor { get; set; } = new Color(1, 1, 1);
    
    protected override unsafe void OnDraw() {
        
        Shader.Get().Use();
        
//        mat4 transformMat = glm.translate(new mat4(1), new vec3(WorldPosition.X, WorldPosition.Y, WorldPosition.Z)) *
//                            glm.rotate(WorldRotation.X / 10, new vec3(1, 0, 0)) *
//                            glm.rotate(WorldRotation.Y / 10, new vec3(0, 1, 0)) *
//                            glm.rotate(WorldRotation.Z / 10, new vec3(0, 0, 1)) *
//                            glm.scale(new mat4(1), new vec3(WorldScale.X, WorldScale.Y, WorldScale.Z));
        
        GlmSharp.mat4 transformMat = GetViewMat();
        
//        mat4 Convert(GlmSharp.mat4 mat4) {
//            return new GlmNet.mat4(
//                new GlmNet.vec4(mat4.m00, mat4.m01, mat4.m02, mat4.m03),
//                new GlmNet.vec4(mat4.m10, mat4.m11, mat4.m12, mat4.m13),
//                new GlmNet.vec4(mat4.m20, mat4.m21, mat4.m22, mat4.m23),
//                new GlmNet.vec4(mat4.m30, mat4.m31, mat4.m32, mat4.m33)
//            );
//        }
        
        GlmSharp.mat4 GetViewMat() {
            GlmSharp.mat4 t = GlmSharp.mat4.Translate(WorldPosition.X, WorldPosition.Y, WorldPosition.Z) *
                              new GlmSharp.quat(WorldRotation.X, WorldRotation.Y, WorldRotation.Z, WorldRotation.W).Normalized.ToMat4;
            //todo: add scale matrix
            return t;
        }
        
        Shader.Get().SetMat("model", transformMat);
        Shader.Get().SetMat("projection", Rendering.Renderer.CurrentCamera.GLM_GetProjectionMatrix());
        Texture.Get().Bind(0);
        Shader.Get().SetInt("u_Texture", 0);
        Shader.Get().SetFloat("time", Time.TotalTimeElapsed);
        Shader.Get().SetVector3("lightDirection", DirectionalLightRotation);
        Shader.Get().SetVector3("directionalLightColor", new Vector3(DirectionalLightColor.R, DirectionalLightColor.G, DirectionalLightColor.B));
        
        Model model = Model.Get();
        
        if(Textures is null)
            return;
        
        Mesh[] meshes = model.Meshes;
        
        for(int i = 0; i < meshes.Length; i++) {
            
            Texture2D texture = Textures.Count > i ? Textures[i].Get() : (Texture2D) Texture2D.Default;
            
            texture.Bind(0);
            
            Gl.BindVertexArray(meshes[i].Vao);
//            if(meshes[i] is PosUvNormalMeshIndexedBuffer posUvNormalGeometryEbo) {
                // indexed drawing
                Gl.DrawElements(PrimitiveType.Triangles, (uint) meshes[i].EboLength, DrawElementsType.UnsignedInt, null); // can't use indices here, just pass in nullptr and it will use last bound
//            } else {
//                // normal drawing
//                Gl.DrawArrays(PrimitiveType.Triangles, 0, (uint) meshes[i].VertexCount);
//            }
            Gl.BindVertexArray(0);
        }
        
    }
    
}
