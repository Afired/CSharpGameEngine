using System.Drawing;
using Silk.NET.OpenGL;

namespace ImGui {
    
    class Program {
        
        static void Main(string[] args) {
            
            RenderingEngine renderThread = new RenderingEngine();
            new Thread(renderThread.StartRenderThread).Start();
        }

    }
    
}
