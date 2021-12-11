using GameEngine.Rendering;
using GLFW;
using OpenGL;

namespace GameEngine.Core; 

public sealed partial class Game {
    
    private void StartRenderThread() {
        Window window = WindowFactory.CreateWindow(900, 600, "Window Title", false);

        while(!Glfw.WindowShouldClose(window)) {
            Glfw.PollEvents();
            Render(window);
        }
        Terminate();
    }
    
    private void Render(Window window) {
        GL.glClearColor(1.0f, 0.0f, 0.0f, 1.0f);
        GL.glClear(GL.GL_COLOR_BUFFER_BIT);
        
        Glfw.SwapBuffers(window);
    }
    
}
