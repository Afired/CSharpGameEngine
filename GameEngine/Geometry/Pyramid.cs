using GameEngine.Components;
using GameEngine.Core;
using GameEngine.Numerics;
using GameEngine.Rendering.Shaders;
using OpenGL;

namespace GameEngine.Geometry; 

public class Pyramid : Component {
    
    public Geometry Geometry { get; set; }
    public Shader Shader { get; set; }
    private uint _vao;
    private uint _vbo;


    public Pyramid(GameObject gameObject) : base(gameObject) {
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
            //walls
            0, 1, 0, 0.5f, 0.5f, 1.5f,   // top
            1, -1, 1, 0.5f, 0.5f, 1.5f,  // bottom right
            -1, -1, 1, 0.5f, 0.5f, 1.5f, // bottom left
            
            0, 1, 0, 0.5f, 1.5f, 0.5f,   // top
            -1, -1, 1, 0.5f, 1.5f, 0.5f,  // bottom right
            -1, -1, -1, 0.5f, 1.5f, 0.5f, // bottom left
            
            0, 1, 0, 1.5f, 0.5f, 0.5f,    // top
            -1, -1, -1, 1.5f, 0.5f, 0.5f, // bottom right
            1, -1, -1, 1.5f, 0.5f, 0.5f,  // bottom left
            
            0, 1, 0, 0.5f, 0.5f, 0.5f,   // top
            1, -1, -1, 0.5f, 0.5f, 0.5f, // bottom right
            1, -1, 1, 0.5f, 0.5f, 0.5f,  // bottom left
            //base
            -1, -1, -1, 0.5f, 0.5f, 0.5f,
            1, -1, -1, 0.5f, 0.5f, 0.5f,
            -1, -1, 1, 0.5f, 0.5f, 0.5f,
            
            1, -1, 1, 0.5f, 0.5f, 0.5f,
            1, -1, -1, 0.5f, 0.5f, 0.5f,
            -1, -1, 1, 0.5f, 0.5f, 0.5f,
        };

        unsafe {
            fixed(float* v = &vertices[0]) {
                GL.glBufferData(GL.GL_ARRAY_BUFFER, sizeof(float) * vertices.Length, v, GL.GL_STATIC_DRAW);
            }
            
            //xyz
            GL.glVertexAttribPointer(0, 3, GL.GL_FLOAT, false, 6 * sizeof(float), (void*) (0 * sizeof(float)));
            GL.glEnableVertexAttribArray(0);
            
            //rgb
            GL.glVertexAttribPointer(1, 3, GL.GL_FLOAT, false, 6 * sizeof(float), (void*) (3 * sizeof(float)));
            GL.glEnableVertexAttribArray(1);
            
            GL.glBindBuffer(GL.GL_ARRAY_BUFFER, 0);
            GL.glBindVertexArray(0);
        }
        
    }
    

    public void OnDraw() {
        ShaderRegister.Get("default").Use();

        ITransform transform = GameObject as ITransform;
        
        Matrix4x4 trans = Matrix4x4.CreateTranslation(transform.Transform.Position.X, transform.Transform.Position.Y, transform.Transform.Position.Z);
        Matrix4x4 sca = Matrix4x4.CreateScale(transform.Transform.Scale.X, transform.Transform.Scale.Y, transform.Transform.Scale.Z);
        Matrix4x4 rot = Matrix4x4.CreateFromQuaternion(transform.Transform.Rotation);
        
        ShaderRegister.Get("default").SetMatrix4x4("model", rot * sca * trans);
        ShaderRegister.Get("default").SetMatrix4x4("projection", Game.CurrentCamera.GetProjectionMatrix());
        
        GL.glBindVertexArray(_vao);
        GL.glDrawArrays(GL.GL_TRIANGLES, 0, 18);
        GL.glBindVertexArray(0);
    }
    
}
