using GameEngine.Rendering;
using GameEngine.Rendering.Shaders;
using GLFW;
using OpenGL;

namespace GameEngine.Core; 

public sealed partial class Game {
    
    private void StartRenderThread() {
        Window window = WindowFactory.CreateWindow(900, 600, "Window Title", false);
        
        Load();

        while(!Glfw.WindowShouldClose(window)) {
            Glfw.PollEvents();
            Render(window);
        }
        Terminate();
    }
    
    private uint _vao;
    private uint _vbo;
    private Shader _shader;
    
    private const string VERTEX_SHADER = @"#version 330 core
                                    layout (location = 0) in vec2 aPosition;
                                    layout (location = 1) in vec3 aColor;
                                    out vec4 vertexColor;
    
                                    void main() 
                                    {
                                        vertexColor = vec4(aColor.rgb, 1.0);
                                        gl_Position = vec4(aPosition.xy, 0, 1.0);
                                    }";

    private const string FRAGMENT_SHADER = @"#version 330 core
                                    out vec4 FragColor;
                                    in vec4 vertexColor;

                                    void main() 
                                    {
                                        FragColor = vertexColor;
                                    }";

    private void Load() {
        _shader = new Shader(VERTEX_SHADER, FRAGMENT_SHADER);
        _shader.Load();

        _vao = GL.glGenVertexArray();
        _vbo = GL.glGenBuffer();
        
        GL.glBindVertexArray(_vao);
        GL.glBindBuffer(GL.GL_ARRAY_BUFFER, _vbo);
        
        float[] vertices =
        {
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

    private void Render(Window window) {
        GL.glClearColor(0.0f, 0.0f, 0.0f, 1.0f);
        GL.glClear(GL.GL_COLOR_BUFFER_BIT);
        
        _shader.Use();
        
        GL.glBindVertexArray(_vao);
        GL.glDrawArrays(GL.GL_TRIANGLES, 0, 6);
        GL.glBindVertexArray(0);
        
        Glfw.SwapBuffers(window);
    }
    
}
