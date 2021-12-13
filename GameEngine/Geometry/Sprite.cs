using GameEngine.Core;
using GameEngine.Numerics;
using GameEngine.Rendering.Shaders;
using OpenGL;

namespace GameEngine.Geometry; 

public class Sprite : ITransform, IGeometry, IRendered {
    
    public Transform Transform { get; set; }
    public Geometry Geometry { get; set; }
    public Shader Shader { get; set; }
    private uint _vao;
    private uint _vbo;


    public Sprite() {
        Transform = new Transform();
        Game.OnDraw += OnDraw;
        Game.OnLoad += OnLoad;
    }

    private void OnLoad() {
        Shader = ShaderRegister.Get("default");
        InitializeGeometry();
    }

    private void InitializeGeometry() {
        _vao = GL.glGenVertexArray();
        _vbo = GL.glGenBuffer();
        
        GL.glBindVertexArray(_vao);
        GL.glBindBuffer(GL.GL_ARRAY_BUFFER, _vbo);
        
        float[] vertices = {
            -0.5f, 0.5f, 1f, 0f, 0f,  // top left
            0.5f, 0.5f, 0f, 1f, 0f,   // top right
            -0.5f, -0.5f, 0f, 0f, 1f, // bottom left

            0.5f, 0.5f, 0f, 1f, 0f,   // top right
            0.5f, -0.5f, 0f, 1f, 1f,  // bottom right
            -0.5f, -0.5f, 0f, 0f, 1f, // bottom left
        };

        unsafe {
            fixed(float* v = &vertices[0]) {
                GL.glBufferData(GL.GL_ARRAY_BUFFER, sizeof(float) * vertices.Length, v, GL.GL_STATIC_DRAW);
            }
            
            GL.glVertexAttribPointer(0, 2, GL.GL_FLOAT, false, 5 * sizeof(float), (void*) (0 * sizeof(float)));
            GL.glEnableVertexAttribArray(0);
            
            GL.glVertexAttribPointer(1, 3, GL.GL_FLOAT, false, 5 * sizeof(float), (void*) (2 * sizeof(float)));
            GL.glEnableVertexAttribArray(1);
            
            GL.glBindBuffer(GL.GL_ARRAY_BUFFER, 0);
            GL.glBindVertexArray(0);
        }
        
    }
    

    public void OnDraw() {
        ShaderRegister.Get("default").Use();
        
        Matrix4x4 trans = Matrix4x4.CreateTranslation(Transform.Position.X, Transform.Position.Y, Transform.Position.Z);
        Matrix4x4 sca = Matrix4x4.CreateScale(Transform.Scale.X, Transform.Scale.Y, Transform.Scale.Z);
        Matrix4x4 rot = Matrix4x4.CreateRotationZ(0);
        
        ShaderRegister.Get("default").SetMatrix4x4("model", sca * rot * trans);
        ShaderRegister.Get("default").SetMatrix4x4("projection", Game.CurrentCamera.GetProjectionMatrix());
        
        GL.glBindVertexArray(_vao);
        GL.glDrawArrays(GL.GL_TRIANGLES, 0, 6);
        GL.glBindVertexArray(0);
    }
    
}
