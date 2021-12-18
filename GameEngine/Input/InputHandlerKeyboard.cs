using System;
using GLFW;

namespace GameEngine.Input; 

internal partial class InputHandler {
    
    internal void OnKeyAction(IntPtr window, Keys key, int scancode, InputState state, ModifierKeys mods) {
        Input.SetKeyState(key, state != InputState.Release);
        // possible input states:
        // Press -> true
        // Release -> false
        // Repeat -> true
    }
    
}
