using System.Drawing;
using Silk.NET.OpenGL;

namespace ImGui; 

public class RenderingEngine {
    
    public unsafe void StartRenderThread() {
        using GlfwWindow glfwWindow = new GlfwWindow();

        while(!glfwWindow.Glfw.WindowShouldClose(glfwWindow.Handle)) {
            Thread.Sleep(TimeSpan.FromSeconds(0.01f));
            // Make sure ImGui is up-to-date
            glfwWindow.Glfw.PollEvents();
            glfwWindow.ImGuiController.Update(0.01f);
        
            // This is where you'll do any rendering beneath the ImGui context
            // Here, we just have a blank screen.
            glfwWindow.Gl.ClearColor(Color.FromArgb(255, (int) (.45f * 255), (int) (.55f * 255), (int) (.60f * 255)));
            glfwWindow.Gl.Clear((uint) ClearBufferMask.ColorBufferBit);
        
            // This is where you'll do all of your ImGUi rendering
            // Here, we're just showing the ImGui built-in demo window.
            ImGuiNET.ImGui.ShowDemoWindow();
        
            // Make sure ImGui renders too!
            glfwWindow.ImGuiController.Render();
            glfwWindow.Glfw.SwapBuffers(glfwWindow.Handle);
        }
        
        //Terminate
        glfwWindow.Dispose();
    }

}
