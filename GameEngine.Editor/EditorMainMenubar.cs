using System.Numerics;
using System.Runtime.InteropServices;
using GameEngine.Core.Core;
using GameEngine.Core.Rendering.Shaders;
using GameEngine.Core.Rendering.Textures;
using GameEngine.Core.SceneManagement;
using GameEngine.Core.Serialization;
using ImGuiNET;
using Silk.NET.GLFW;

namespace GameEngine.Editor.EditorWindows; 

public class EditorMainMenubar {
    
    private struct Position {
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
    
    public EditorMainMenubar() {
        Program.EditorLayer.OnDraw += Draw;
    }
    
    private void Draw() {
        ImGui.PushStyleVar(ImGuiStyleVar.FramePadding, new Vector2(8, 8));
        ImGui.PushStyleColor(ImGuiCol.MenuBarBg, new Vector4(0.11f, 0.11f, 0.11f, 1.0f));
        if(ImGui.BeginMainMenuBar()) {
            
            DrawAppIcon("Checkerboard");
            
            DrawMenuItems();
            
            DrawWindowHandleButtons();

            InstallDragArea();

            ImGui.EndMainMenuBar();   
        }
        // pop main menu bar size
        ImGui.PopStyleVar();
        ImGui.PopStyleColor();
    }
    
    public const int WM_NCLBUTTONDOWN = 0xA1;
    public const int HTCAPTION = 0x2;

    [DllImport("User32.dll")]
    public static extern bool ReleaseCapture();

    [DllImport("User32.dll")]
    public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
    
    [DllImport("user32.dll")]
    static extern IntPtr SetCapture(IntPtr hWnd);
    
    [DllImport("user32.dll", EntryPoint = "SetForegroundWindow")]
    static extern bool SetForegroundWindow(IntPtr hWnd);
    
    [DllImport("user32.dll")]
    private static extern IntPtr SetFocus(IntPtr hWnd);
    
    public const int WM_LBUTTONDOWN = 0x201;
    public const int WM_LBUTTONUP = 0x0202;

    private unsafe void InstallDragArea() {
        // push style to make invisible
        ImGui.PushStyleVar(ImGuiStyleVar.Alpha, 0);
        
        ImGui.SetCursorPos(new Vector2(0, 0));
        ImGui.Button("", ImGui.GetWindowSize());
        if(ImGui.IsItemActive()) {
            if(!_dragging) {
                _dragging = true;
//                _mousePosRef = new Position(CursorPosition.GetCursorPosition().X, CursorPosition.GetCursorPosition().Y);
//                Glfw.GetWindowPos(GlfwWindow.Handle, out int x, out int y);
//                _windowPosRef = new Position(x, y);
                
                GlfwNativeWindow test = new GlfwNativeWindow(Silk.NET.GLFW.Glfw.GetApi(), GlfwWindow.Handle);
                IntPtr hwnd = test.Win32.Value.Hwnd;
                ReleaseCapture();
                SendMessage(hwnd, WM_NCLBUTTONDOWN, HTCAPTION, 0);
                SetForegroundWindow(hwnd);
                SetFocus(hwnd);
                SendMessage(hwnd, WM_LBUTTONDOWN, 0, 0);
                SendMessage(hwnd, WM_LBUTTONUP, 0, 0);
            }
//            int offsetX = CursorPosition.GetCursorPosition().X - _mousePosRef.X;
//            int offsetY = CursorPosition.GetCursorPosition().Y - _mousePosRef.Y;
//            Glfw.SetWindowPos(GlfwWindow.Handle, _windowPosRef.X + offsetX, _windowPosRef.Y + offsetY);
        } else {
            _dragging = false;
        }

        //pop button drag area alpha
        ImGui.PopStyleVar();
    }

    private static void DrawWindowHandleButtons() {
        
        ImGui.PushStyleColor(ImGuiCol.Button, new Vector4(0f, 0f, 0f, 0.0f));
        ImGui.PushStyleColor(ImGuiCol.ButtonHovered, new Vector4(1f, 1f, 1f, 0.15f));
        ImGui.PushStyleColor(ImGuiCol.ButtonActive, new Vector4(1f, 1f, 1f, 0.2f));
        ImGui.SetCursorPos(new Vector2(ImGui.GetWindowSize().X - 64, 0));
        Texture2D icon2 = TextureRegister.Get("Box") as Texture2D;
        if(ImGui.ImageButton((IntPtr) icon2.ID, new Vector2(16, 16))) {
            unsafe {
                GlfwWindow.Glfw.MaximizeWindow(GlfwWindow.Handle);
            }
        }
        ImGui.PopStyleColor(3);
        
        
        ImGui.PushStyleColor(ImGuiCol.Button, new Vector4(0f, 0f, 0f, 0.0f));
        ImGui.PushStyleColor(ImGuiCol.ButtonHovered, new Vector4(1f, 0f, 0f, 0.75f));
        ImGui.PushStyleColor(ImGuiCol.ButtonActive, new Vector4(1f, 0.4f, 0.4f, 0.75f));
        ImGui.SetCursorPos(new Vector2(ImGui.GetWindowSize().X - 32, 0));
        Texture2D icon1 = TextureRegister.Get("Checkerboard") as Texture2D;
        if(ImGui.ImageButton((IntPtr) icon1.ID, new Vector2(16, 16))) {
            Application.Terminate();
        }
        ImGui.PopStyleColor(3);
    }

    private static void DrawMenuItems() {
        ImGui.SetCursorPos(new Vector2(32, 0));
        
        if(ImGui.BeginMenu("Application")) {
            if(ImGui.MenuItem("Preferences")) { }
            if(ImGui.MenuItem("Quit")) Application.Terminate();
            ImGui.EndMenu();
        }
        
        if(ImGui.BeginMenu("Project")) {
            if(ImGui.MenuItem("Project Settings")) { new ProjectSettingsWindow(); }
            if(ImGui.MenuItem("Open Project")) { }
            ImGui.EndMenu();
        }
        
        if(ImGui.BeginMenu("Window")) {
            if(ImGui.MenuItem("Asset Browser")) { new AssetBrowserWindow(); }
            if(ImGui.MenuItem("Console")) { new ConsoleWindow(); }
            if(ImGui.MenuItem("Hierarchy")) { new HierarchyWindow(); }
            if(ImGui.MenuItem("Inspector")) { new InspectorWindow(); }
            if(ImGui.MenuItem("Viewport")) { new ViewportWindow(); }
            if(ImGui.MenuItem("Scene Select")) { new SceneSelectWindow(); }
            ImGui.EndMenu();
        }
        
        if(ImGui.BeginMenu("Scene")) {
            if(ImGui.MenuItem("New")) Hierarchy.LoadScene(new Scene());
            if(ImGui.MenuItem("Save")) SceneSerializer.SaveOpenedScene();
            if(ImGui.MenuItem("Load")) Hierarchy.LoadScene(SceneSerializer.LoadJson("Test"));
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
