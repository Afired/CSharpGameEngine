// This file is part of Silk.NET.
//
// You may modify and distribute Silk.NET under the terms
// of the MIT license. See the LICENSE file for details.

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
        public void StartRenderThread() {
            GlfwWindow glfwWindow = new GlfwWindow();
        }
    }
}