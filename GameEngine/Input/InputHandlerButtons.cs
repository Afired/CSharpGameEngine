using System.ComponentModel;
using GLFW;

namespace GameEngine.Input; 

internal partial class InputHandler {

    private bool InterpretState(InputState inputState) {
        switch(inputState) {
            case InputState.Press:
                return true;
            case InputState.Repeat:
                return true;
            case InputState.Release:
                return false;
            default:
                throw new InvalidEnumArgumentException();
        }
    }
    
}
