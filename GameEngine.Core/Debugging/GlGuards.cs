#define DEBUG_GL

global using static GameEngine.Core.Debugging.GlGuards;

using System.Runtime.CompilerServices;
using Silk.NET.OpenGL;

namespace GameEngine.Core.Debugging; 

public static class GlGuards {
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void GlCheckError() {
        #if DEBUG_GL
        Console.LogError("some error in debug build");
        GLEnum err;
        while((err = Gl.GetError()) != GLEnum.NoError) {
            Console.Log(err.ToString());
        }
        #endif
    }
    
}
