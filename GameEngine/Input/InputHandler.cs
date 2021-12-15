using System;
using GLFW;
using Console = GameEngine.Debugging.Console;

namespace GameEngine.Input; 

internal partial class InputHandler {
    
    internal void HandleInput(Window window) {
        HandleButtons(window);
        HandleMouseInput(window);
    }
    
    internal void OnKeyAction(IntPtr window, Keys key, int scancode, InputState state, ModifierKeys mods) {
        Console.Log(key.ToString());
    }

}
