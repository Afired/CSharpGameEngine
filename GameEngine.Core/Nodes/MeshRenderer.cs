using System.Collections.Generic;
using GameEngine.Core.AssetManagement;
using GameEngine.Core.Numerics;
using GameEngine.Core.Rendering;
using GameEngine.Core.Rendering.Geometry;
using GameEngine.Core.Rendering.Textures;
using GameEngine.Core.Serialization;
using Silk.NET.OpenGL;
using Shader = GameEngine.Core.Rendering.Shaders.Shader;

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
        
        Shader shader = Shader.Get() ?? Rendering.Shaders.Shader.Invalid;
        Texture2D texture2D = Texture.Get() ?? Texture2D.Missing;
        
        shader.Use();
        
        GlmSharp.mat4 transformMat = GetViewMat();
        
        GlmSharp.mat4 GetViewMat() {
            GlmSharp.mat4 t = GlmSharp.mat4.Translate(WorldPosition.X, WorldPosition.Y, WorldPosition.Z) *
                              new GlmSharp.quat(WorldRotation.X, WorldRotation.Y, WorldRotation.Z, WorldRotation.W).Normalized.ToMat4;
            //todo: add scale matrix
            return t;
        }
        
        shader.SetMat("model", transformMat);
        shader.SetMat("projection", Rendering.Renderer.CurrentCamera.GLM_GetProjectionMatrix());
        texture2D.Bind(0);
        shader.SetInt("u_Texture", 0);
        shader.SetFloat("time", Time.TotalTimeElapsed);
        shader.SetVector3("lightDirection", DirectionalLightRotation);
        shader.SetVector3("directionalLightColor", new Vector3(DirectionalLightColor.R, DirectionalLightColor.G, DirectionalLightColor.B));
        
        Model model = Model.Get() ?? Rendering.Geometry.Model.Empty;
        
        if(Textures is null)
            return;
        
        Mesh[] meshes = model.Meshes;
        
        for(int i = 0; i < meshes.Length; i++) {
            
            Texture2D texture = Textures.Count > i ? Textures[i].Get() ?? Texture2D.Missing : Texture2D.Missing;
            
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
