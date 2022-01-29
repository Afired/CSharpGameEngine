using System.Numerics;
using System.Runtime.InteropServices;
using Editor;
using ExampleGame.Entities;
using GameEngine.Core;
using GameEngine.Rendering;
using GameEngine.Rendering.Shaders;
using GameEngine.Rendering.Textures;
using GameEngine.SceneManagement;
using ImGuiNET;
using Silk.NET.GLFW;
using Vector3 = GameEngine.Numerics.Vector3;

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
        Program.EditorLayer.OnDraw += Draw;
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
        
        if(ImGui.BeginMenu("File")) {
            if(ImGui.MenuItem("Open Scene")) {
                Scene scene = new Scene();
                scene.Name = "Test Scene";
                
                scene.AddEntity(new PhysicsQuad() {
                    Transform = { Position = new Vector3(0, 10, 0)},
                    Renderer = { Texture = "Box", Shader = "default"}
                });
                scene.AddEntity(new PhysicsQuad() {
                    Transform = { Position = new Vector3(0.5f, 11, 0)},
                    Renderer = { Texture = "Checkerboard", Shader = "default"}
                });
                scene.AddEntity(new PhysicsQuad() {
                    Transform = { Position = new Vector3(-0.25f, 12, 0)},
                    Renderer = { Texture = "Checkerboard", Shader = "default"}
                });
                scene.AddEntity(new Quad() {
                    Transform = { Position = new Vector3(0, 2, 0)} ,
                    Renderer = { Texture = "Checkerboard", Shader = "default" }
                });
                Player player = new Player();
                RenderingEngine.SetActiveCamera(player.Camera2D);
                scene.AddEntity(player);
                
                Hierarchy.LoadScene(scene);
            }
            if(ImGui.MenuItem("Save Scene")) { }
            ImGui.EndMenu();
        }
        
        if(ImGui.BeginMenu("Project")) {
            if(ImGui.MenuItem("Project Settings")) { }
            if(ImGui.MenuItem("Open Project")) { }
            ImGui.EndMenu();
        }
        
        if(ImGui.BeginMenu("Windows")) {
            if(ImGui.MenuItem("AssetBrowser")) { }
            if(ImGui.MenuItem("Console")) { }
            if(ImGui.MenuItem("Hierarchy")) { }
            if(ImGui.MenuItem("Inspector")) { }
            if(ImGui.MenuItem("Viewport")) { }
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
