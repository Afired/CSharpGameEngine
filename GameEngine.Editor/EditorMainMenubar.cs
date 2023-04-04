using System.Numerics;
using System.Runtime.InteropServices;
using GameEngine.Core;
using GameEngine.Core.AssetManagement;
using GameEngine.Core.Nodes;
using GameEngine.Core.Rendering;
using GameEngine.Core.Rendering.Geometry;
using GameEngine.Core.Rendering.Shaders;
using GameEngine.Core.Rendering.Textures;
using GameEngine.Core.SceneManagement;
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
    
    private static bool _dragging;
    
    public EditorMainMenubar() {
        EditorApplication.Instance.EditorLayer.OnDraw += Draw;
    }
    
    internal static void Draw(Renderer renderer) {
        ImGui.PushStyleVar(ImGuiStyleVar.FramePadding, new Vector2(8, 8));
        ImGui.PushStyleColor(ImGuiCol.MenuBarBg, new Vector4(0.11f, 0.11f, 0.11f, 1.0f));
        //TODO: NoNav
        if(ImGui.BeginMainMenuBar()) {
            
            DrawPlayControls();
            
            Texture2D appIcon = EditorResources.GetIcon("AppIcon");
            ImGui.SetCursorPos(new Vector2(8, 8));
            ImGui.Image((nint) appIcon.Id, new Vector2(16, 16));
            
            DrawMenuItems();
            
            if(ImGui.Button("recompile external assemblies"))
                EditorApplication.Instance.RegisterReloadOfExternalAssemblies();
            
            DrawWindowHandleButtons(renderer);
            
            InstallDragArea(renderer);
            
            ImGui.EndMainMenuBar();   
        }
        // pop main menu bar size
        ImGui.PopStyleVar();
        ImGui.PopStyleColor();
    }
    
    private static void DrawPlayControls() {
        
        void AlignForWidth(float width, float alignment = 0.5f) {
            float avail = ImGui.GetContentRegionAvail().X;
            float off = (avail - width) * alignment;
            if(off > 0.0f)
                ImGui.SetCursorPosX(ImGui.GetCursorPosX() + off);
        }
        
        ImGuiStylePtr style = ImGui.GetStyle();
        float width = 0.0f;
        
        switch(PlayMode.Current) {
            case PlayMode.Mode.Editing:
                width += ImGui.CalcTextSize("Play").X;
                AlignForWidth(width);
                
                if(ImGui.Button("Play"))
                    PlayMode.Start();
                return;
            case PlayMode.Mode.Playing:
                width += ImGui.CalcTextSize("Pause").X;
                width += style.ItemSpacing.X;
                width += ImGui.CalcTextSize("Stop").X;
                AlignForWidth(width);
                
                if(ImGui.Button("Pause"))
                    PlayMode.Pause();
                if(ImGui.Button("Stop"))
                    PlayMode.Stop();
                return;
            case PlayMode.Mode.Paused:
                width += ImGui.CalcTextSize("Resume").X;
                width += style.ItemSpacing.X;
                width += ImGui.CalcTextSize("Stop").X;
                AlignForWidth(width);
                
                if(ImGui.Button("Resume"))
                    PlayMode.Resume();
                if(ImGui.Button("Stop"))
                    PlayMode.Stop();
                return;
            default:
                throw new ArgumentOutOfRangeException();
        }
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
    
    private static unsafe void InstallDragArea(Renderer renderer) {
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
                
                GlfwNativeWindow test = new GlfwNativeWindow(Silk.NET.GLFW.Glfw.GetApi(), renderer.MainWindow.Handle);
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
    
    private static void DrawWindowHandleButtons(Renderer renderer) {
        
        ImGui.PushID("minimize");
        ImGui.PushStyleColor(ImGuiCol.Button, new Vector4(0f, 0f, 0f, 0.0f));
        ImGui.PushStyleColor(ImGuiCol.ButtonHovered, new Vector4(1f, 1f, 1f, 0.15f));
        ImGui.PushStyleColor(ImGuiCol.ButtonActive, new Vector4(1f, 1f, 1f, 0.2f));
        ImGui.SetCursorPos(new Vector2(ImGui.GetWindowSize().X - 96, 0));
        Texture2D minimizeIcon = EditorResources.GetIcon("MinimizeIcon");
        if(ImGui.ImageButton((IntPtr) minimizeIcon.Id, new Vector2(16, 16))) {
            unsafe {
                renderer.MainWindow.Glfw.IconifyWindow(renderer.MainWindow.Handle);
            }
        }
        ImGui.PopStyleColor(3);
        ImGui.PopID();
        
        ImGui.PushID("maximize");
        ImGui.PushStyleColor(ImGuiCol.Button, new Vector4(0f, 0f, 0f, 0.0f));
        ImGui.PushStyleColor(ImGuiCol.ButtonHovered, new Vector4(1f, 1f, 1f, 0.15f));
        ImGui.PushStyleColor(ImGuiCol.ButtonActive, new Vector4(1f, 1f, 1f, 0.2f));
        ImGui.SetCursorPos(new Vector2(ImGui.GetWindowSize().X - 64, 0));
        Texture2D fullscreenIcon = EditorResources.GetIcon("MaximizeIcon");
        if(ImGui.ImageButton((IntPtr) fullscreenIcon.Id, new Vector2(16, 16))) {
            unsafe {
                renderer.MainWindow.Glfw.MaximizeWindow(renderer.MainWindow.Handle);
            }
        }
        ImGui.PopStyleColor(3);
        ImGui.PopID();
        
        ImGui.PushID("close");
        ImGui.PushStyleColor(ImGuiCol.Button, new Vector4(0f, 0f, 0f, 0.0f));
        ImGui.PushStyleColor(ImGuiCol.ButtonHovered, new Vector4(1f, 0f, 0f, 0.75f));
        ImGui.PushStyleColor(ImGuiCol.ButtonActive, new Vector4(1f, 0.4f, 0.4f, 0.75f));
        ImGui.SetCursorPos(new Vector2(ImGui.GetWindowSize().X - 32, 0));
        Texture2D exitIcon = EditorResources.GetIcon("ExitIcon");
        if(ImGui.ImageButton((IntPtr) exitIcon.Id, new Vector2(16, 16))) {
            Application.Instance.Terminate();
        }
        ImGui.PopStyleColor(3);
        ImGui.PopID();
    }

    private static void DrawMenuItems() {
        ImGui.SetCursorPos(new Vector2(32, 0));
        
        if(ImGui.BeginMenu("Application")) {
            if(ImGui.MenuItem("Preferences")) { }
            // if(ImGui.MenuItem("Clear Editor Resources")) ExternalEditorAssemblyManager.ClearEditorResources();
            // if(ImGui.MenuItem("Generate Editor Resources")) ExternalEditorAssemblyManager.GenerateEditorResources();
            // if(ImGui.MenuItem("Reload Editor Assemblies")) ExternalEditorAssemblyManager.RegisterReloadOfEditorAssemblies();
            // if(ImGui.MenuItem("Unload Editor Assemblies")) ExternalEditorAssemblyManager.TryToUnloadEditorAssemblies();
            // if(ImGui.MenuItem("Load Editor Assemblies")) ExternalEditorAssemblyManager.LoadEditorAssemblies();
            if(ImGui.MenuItem("Quit")) Application.Instance.Terminate();
            ImGui.EndMenu();
        }
        
        if(ImGui.BeginMenu("Project")) {
            if(ImGui.MenuItem("Project Settings")) { EditorWindow.Create<ProjectSettingsWindow>(); }
            if(ImGui.MenuItem("Reload Asset Database")) { AssetDatabase.Reload(Application.Instance); }
            if(ImGui.MenuItem("Open Project")) { Project.OpenProjectWithFileExplorer(); }
            ImGui.EndMenu();
        }
        
        if(ImGui.BeginMenu("Window")) {
            if(ImGui.MenuItem("Asset Browser")) EditorWindow.Create<AssetBrowserWindow>();
            if(ImGui.MenuItem("Console")) EditorWindow.Create<ConsoleWindow>();
            if(ImGui.MenuItem("Hierarchy")) EditorWindow.Create<HierarchyWindow>();
            if(ImGui.MenuItem("Inspector")) EditorWindow.Create<InspectorWindow>();
            if(ImGui.MenuItem("Viewport")) EditorGui.Instance.AddWindow(new ViewportWindow(Application.Instance.Renderer.MainWindow.Gl, Application.Instance.Renderer));
            if(ImGui.MenuItem("Terminal")) EditorWindow.Create<TerminalWindow>();
            ImGui.EndMenu();
        }
        
        if(ImGui.BeginMenu("Hierarchy")) {
            if(ImGui.MenuItem("New Scene")) {
                Console.LogWarning("New scene functionality is currently unstable!");
                Hierarchy.Open(Node.New<Scene>());
            }
            if(ImGui.MenuItem("Save"))
                Hierarchy.SaveCurrentRootNode();
            ImGui.EndMenu();
        }
        
        //ImGui.Text(CursorPosition.GetCursorPosition().X + " " + CursorPosition.GetCursorPosition().Y);
    }
    
}
