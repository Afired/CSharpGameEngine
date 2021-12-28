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
    
    private void Draw() {
        ImGui.PushStyleVar(ImGuiStyleVar.FramePadding, new Vector2(8, 8));
        if(ImGui.BeginMainMenuBar()) {
            
            DrawAppIcon("Checkerboard");
            
            DrawMenuItems();
            
            DrawWindowHandleButtons();

            InstallDragArea();

            ImGui.EndMainMenuBar();   
        }
        // pop main menu bar size
        ImGui.PopStyleVar();
    }

    private unsafe void InstallDragArea() {
        // push style to make invisible
        ImGui.PushStyleVar(ImGuiStyleVar.Alpha, 0);
        
        ImGui.SetCursorPos(new Vector2(0, 0));
        ImGui.Button("", ImGui.GetWindowSize());
        if(ImGui.IsItemActive()) {
            if(!_dragging) {
                _dragging = true;
                _mousePosRef = new Position(CursorPosition.GetCursorPosition().X, CursorPosition.GetCursorPosition().Y);
                Glfw.GetWindowPos(GlfwWindow.Handle, out int x, out int y);
                _windowPosRef = new Position(x, y);
            }
            int offsetX = CursorPosition.GetCursorPosition().X - _mousePosRef.X;
            int offsetY = CursorPosition.GetCursorPosition().Y - _mousePosRef.Y;
            Glfw.SetWindowPos(GlfwWindow.Handle, _windowPosRef.X + offsetX, _windowPosRef.Y + offsetY);
        } else {
            _dragging = false;
        }

        //pop button drag area alpha
        ImGui.PopStyleVar();
    }

    private static void DrawWindowHandleButtons() {
        ImGui.SetCursorPos(new Vector2(ImGui.GetWindowSize().X - 32, 0));
        Texture2D icon = TextureRegister.Get("Checkerboard") as Texture2D;
        if(ImGui.ImageButton((IntPtr) icon.ID, new Vector2(16, 16))) {
            Application.Terminate();
        }
    }

    private static void DrawMenuItems() {
        ImGui.SetCursorPos(new Vector2(32, 0));
        
        if(ImGui.BeginMenu("Application")) {
            if(ImGui.MenuItem("Quit")) Application.Terminate();
            ImGui.EndMenu();
        }
        
        if(ImGui.BeginMenu("Project")) {
            if(ImGui.MenuItem("Settings")) { }
            if(ImGui.MenuItem("Open")) { }
            ImGui.EndMenu();
        }
        
        if(ImGui.BeginMenu("Windows")) {
            if(ImGui.MenuItem("AssetBrowser")) new AssetBrowserWindow();
            if(ImGui.MenuItem("Console")) new ConsoleWindow();
            if(ImGui.MenuItem("Hierarchy")) new HierarchyWindow();
            if(ImGui.MenuItem("Inspector")) new InspectorWindow();
            if(ImGui.MenuItem("Viewport")) new ViewportWindow();
            ImGui.EndMenu();
        }

        ImGui.Text(CursorPosition.GetCursorPosition().X + " " + CursorPosition.GetCursorPosition().Y);
    }

    private static void DrawAppIcon(string iconName) {
        Texture2D icon = TextureRegister.Get(iconName) as Texture2D;
        ImGui.SetCursorPos(new Vector2(8, 8));
        ImGui.Image((IntPtr) icon.ID, new Vector2(16, 16));
    }
}
