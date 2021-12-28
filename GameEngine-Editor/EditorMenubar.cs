using System.Numerics;
using Editor;
using GameEngine.Core;
using GameEngine.Rendering.Shaders;
using GameEngine.Rendering.Textures;
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
        ImGui.PushStyleVar(ImGuiStyleVar.FramePadding, new Vector2(8, 8));
        if(ImGui.BeginMainMenuBar()) {

            Texture2D texture = TextureRegister.Get("Checkerboard") as Texture2D;
            ImGui.SetCursorPos(new Vector2(8, 8));
            ImGui.Image((IntPtr) texture.ID, new Vector2(16, 16));
            
            ImGui.SetCursorPos(new Vector2(32, 0));
            
            // start draw menu items
            if(ImGui.BeginMenu("Application")) {
                if(ImGui.MenuItem("Quit"))
                    Application.Terminate();
                ImGui.EndMenu();
            } // end draw menu items
            
            ImGui.Text(CursorPosition.GetCursorPosition().X + " " + CursorPosition.GetCursorPosition().Y);
            
            ImGui.SetCursorPos(new Vector2(ImGui.GetWindowSize().X - 32, 0));
            if(ImGui.ImageButton((IntPtr) texture.ID, new Vector2(16, 16))) {
                Application.Terminate();
            }

            //push style to make invisible
            //ImGui.PushStyleColor(ImGuiCol.Button, new Vector4(0, 0, 0, 0));
            //ImGui.PushStyleColor(ImGuiCol.ButtonHovered, new Vector4(0, 0, 0, 0));
            //ImGui.PushStyleColor(ImGuiCol.ButtonActive, new Vector4(0, 0, 0, 0));
            ImGui.PushStyleVar(ImGuiStyleVar.Alpha, 0);
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
            //pop button drag area alpha
            ImGui.PopStyleVar();

            ImGui.EndMainMenuBar();   
        }
        //pop main menu bar size
        ImGui.PopStyleVar();
        
    }
    
}
