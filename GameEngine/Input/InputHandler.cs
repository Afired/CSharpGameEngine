using GameEngine.Debugging;
using GLFW;

namespace GameEngine.Input; 

internal partial class InputHandler {
    
    internal void HandleInput(Window window) {
        HandleButtons(window);
        HandleMouseInput(window);
    }

}
