// This file is part of Silk.NET.
//
// You may modify and distribute Silk.NET under the terms
// of the MIT license. See the LICENSE file for details.

using System.Drawing;
using Silk.NET.GLFW;
using Silk.NET.Windowing;
using Silk.NET.Input;
using Silk.NET.Maths;
using Silk.NET.OpenGL;
using Silk.NET.OpenGL.Extensions.ImGui;
using Silk.NET.Windowing.Glfw;
using VideoMode = Silk.NET.Windowing.VideoMode;

namespace ImGui
{
    
    class Program
    {
        static void Main(string[] args) {
            RenderThread renderThread = new RenderThread();
            new Thread(renderThread.StartRenderThread).Start();
        }

    }

    class RenderThread {
        
        public unsafe void StartRenderThread() {
            using GlfwWindow glfwWindow = new GlfwWindow();

            while(!glfwWindow.Glfw.WindowShouldClose(glfwWindow.Handle)) {
                Thread.Sleep(TimeSpan.FromSeconds(0.1f));
                Console.WriteLine("Frame");
                // Make sure ImGui is up-to-date
                glfwWindow.Glfw.PollEvents();
                glfwWindow.ImGuiController.Update(0.1f);
        
                // This is where you'll do any rendering beneath the ImGui context
                // Here, we just have a blank screen.
                glfwWindow.Gl.ClearColor(Color.FromArgb(255, (int) (.45f * 255), (int) (.55f * 255), (int) (.60f * 255)));
                glfwWindow.Gl.Clear((uint) ClearBufferMask.ColorBufferBit);
        
                // This is where you'll do all of your ImGUi rendering
                // Here, we're just showing the ImGui built-in demo window.
                ImGuiNET.ImGui.ShowDemoWindow();
        
                // Make sure ImGui renders too!
                glfwWindow.ImGuiController.Render();
                unsafe {
                    glfwWindow.Glfw.SwapBuffers(glfwWindow.Handle);
                }
            }
            
            //Terminate
        }
        
    }
}