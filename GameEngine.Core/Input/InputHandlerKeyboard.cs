using System;
using Silk.NET.GLFW;

namespace GameEngine.Core.Input; 

public partial class InputHandler {
    
    internal unsafe void OnKeyAction(WindowHandle* window, Keys key, int scancode, InputAction state, KeyModifiers mods) {
        Input.SetKeyState(key, state != InputAction.Release);
        // possible input states:
        // Press -> true
        // Release -> false
        // Repeat -> true
    }
    
}
