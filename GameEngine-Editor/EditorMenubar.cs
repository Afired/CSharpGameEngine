using System.Numerics;
using Editor;
using GameEngine.Core;
using ImGuiNET;

namespace GameEngine.Editor.EditorWindows; 

public class EditorMenubar {

    struct Position {
        public int X;
        public int Y;
        public Position(int x, int y) {
            X = x;
            Y = y;
        }
    }

    private Position _windowPosRef;
    private Position _mousePosRef;

    private bool _dragging;
    
    public EditorMenubar() {
        OnImGui += Draw;
    }

    private double lastMousePositionX;
    private double lastMousePositionY;
    

    private void Draw() {
        if(ImGui.BeginMainMenuBar()) {

            if(ImGui.BeginMenu("Application")) {
                
                if(ImGui.MenuItem("Quit")) {
                    Application.Terminate();
                }

                ImGui.EndMenu();
            }
            
            ImGui.Text(CursorPosition.GetCursorPosition().X + " " + CursorPosition.GetCursorPosition().Y);
            
            //push style to make invisible
            ImGui.PushStyleColor(ImGuiCol.Button, new Vector4(0, 0, 0, 0));
            ImGui.PushStyleColor(ImGuiCol.ButtonHovered, new Vector4(0, 0, 0, 0));
            ImGui.PushStyleColor(ImGuiCol.ButtonActive, new Vector4(0, 0, 0, 0));
            ImGui.SetCursorPos(new Vector2(0, 0));
            ImGui.Button("", ImGui.GetWindowSize());
            if(ImGui.IsItemActive()) {
                if(!_dragging) {
                    _dragging = true;
                    _mousePosRef = new Position(CursorPosition.GetCursorPosition().X, CursorPosition.GetCursorPosition().Y);
                    unsafe {
                        Glfw.GetWindowPos(GlfwWindow.Handle, out int x, out int y);
                        _windowPosRef = new Position(x, y);
                    }
                }
                unsafe {
                    int offsetX = CursorPosition.GetCursorPosition().X - _mousePosRef.X;
                    int offsetY = CursorPosition.GetCursorPosition().Y - _mousePosRef.Y;
                    Glfw.SetWindowPos(GlfwWindow.Handle, _windowPosRef.X + offsetX, _windowPosRef.Y + offsetY);
                }
            } else {
                _dragging = false;
            }

            ImGui.EndMainMenuBar();   
        }
        
    }
    
}
