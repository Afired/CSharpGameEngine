using GameEngine.Core.Numerics;
using GameEngine.Core.Rendering.Geometry;
using GameEngine.Core.Rendering.Shaders;
using GameEngine.Core.Serialization;
using GlmNet;
using Silk.NET.OpenGL;

namespace GameEngine.Core.Nodes; 

public partial class MeshRenderer : Transform {
    
    [Serialized] public string Texture { get; set; } = "checkerboard";
    [Serialized] public string Shader { get; set; } = "default";
    [Serialized] public string Geometry { get; set; } = "car";
    
    [Serialized] public Vector3 Rotation3D { get; private set; }
    
    protected override void OnDraw() {
        ShaderRegister.Get(Shader).Use();
        
        mat4 transformMat = glm.translate(new mat4(1), new vec3(Position.X, Position.Y, Position.Z)) *
                            glm.rotate(Rotation3D.X / 10, new vec3(1, 0, 0)) *
                            glm.rotate(Rotation3D.Y / 10, new vec3(0, 1, 0)) *
                            glm.rotate(Rotation3D.Z / 10, new vec3(0, 0, 1)) *
                            glm.scale(new mat4(1), new vec3(Scale.X, Scale.Y, Scale.Z));
        
        ShaderRegister.Get(Shader).GLM_SetMat("model", transformMat);
        ShaderRegister.Get(Shader).GLM_SetMat("projection", Rendering.Renderer.CurrentCamera.GLM_GetProjectionMatrix());
        
        Geometry geometry = GeometryRegister.Get(Geometry);
        if(geometry is null) {
            return;
        }
        
        Gl.BindVertexArray(geometry.Vao);
        
        TextureRegister.Get(Texture).Bind();
        ShaderRegister.Get(Shader).SetInt("u_Texture", 0);
        
        GLEnum err;
        while((err = Gl.GetError()) != GLEnum.NoError) {
            Console.LogError(err.ToString());
        }
        Console.Log("_______________");
        
        // normal drawing
        // Gl.DrawArrays(PrimitiveType.Triangles, 0, (uint) geometry.VertexCount);
        // indexed drawing - currently doesnt work :/
        Gl.DrawElements(PrimitiveType.Triangles, (uint) (geometry as PosUvNormalGeometryIndexedBuffer).EboLength * 3, DrawElementsType.UnsignedInt, 0);
        
        Gl.BindVertexArray(0);
    }
    
}
