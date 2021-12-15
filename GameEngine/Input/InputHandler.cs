using System;
using GLFW;

namespace GameEngine.Input; 

internal partial class InputHandler {
    
    internal void HandleInput(Window window) {
        HandleMouseInput(window);
    }
    
    internal void OnKeyAction(IntPtr window, Keys key, int scancode, InputState state, ModifierKeys mods) {
        Input.SetKeyState(key, state == InputState.Press);
    }

}
