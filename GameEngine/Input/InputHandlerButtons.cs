using System.ComponentModel;
using GLFW;

namespace GameEngine.Input; 

internal partial class InputHandler {

    private void HandleButtons(Window window) {
        SetKeyState(window, Keys.A, KeyCode.A);
        SetKeyState(window, Keys.B, KeyCode.B);
        SetKeyState(window, Keys.C, KeyCode.C);
        SetKeyState(window, Keys.D, KeyCode.D);
        SetKeyState(window, Keys.E, KeyCode.E);
        SetKeyState(window, Keys.F, KeyCode.F);
        SetKeyState(window, Keys.G, KeyCode.G);
        SetKeyState(window, Keys.H, KeyCode.H);
        SetKeyState(window, Keys.I, KeyCode.I);
        SetKeyState(window, Keys.J, KeyCode.J);
        SetKeyState(window, Keys.K, KeyCode.K);
        SetKeyState(window, Keys.L, KeyCode.L);
        SetKeyState(window, Keys.M, KeyCode.M);
        SetKeyState(window, Keys.N, KeyCode.N);
        SetKeyState(window, Keys.O, KeyCode.O);
        SetKeyState(window, Keys.P, KeyCode.P);
        SetKeyState(window, Keys.Q, KeyCode.Q);
        SetKeyState(window, Keys.R, KeyCode.R);
        SetKeyState(window, Keys.S, KeyCode.S);
        SetKeyState(window, Keys.T, KeyCode.T);
        SetKeyState(window, Keys.U, KeyCode.U);
        SetKeyState(window, Keys.V, KeyCode.V);
        SetKeyState(window, Keys.W, KeyCode.W);
        SetKeyState(window, Keys.X, KeyCode.X);
        SetKeyState(window, Keys.Y, KeyCode.Y);
        SetKeyState(window, Keys.Z, KeyCode.Z);
        SetKeyState(window, Keys.Space, KeyCode.Space);
        SetKeyState(window, Keys.Escape, KeyCode.Escape);
    }
    
    private void SetKeyState(Window window, Keys key, KeyCode keyCode) {
        Input.SetKeyState(keyCode, InterpretState(Glfw.GetKey(window, key)));
    }

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
