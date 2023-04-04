using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;
using GameEngine.Core.Rendering;
using GameEngine.Editor.EditorWindows;
using ImGuiNET;
using Silk.NET.GLFW;
using Silk.NET.OpenGL;

namespace GameEngine.Editor; 

public unsafe delegate void Platform_CreateWindow(ImGuiViewportPtr vp);
public unsafe delegate void Platform_DestroyWindow(ImGuiViewportPtr vp);
public unsafe delegate void Platform_GetWindowPos(ImGuiViewportPtr vp, Vector2* outPos);
public unsafe delegate void Platform_ShowWindow(ImGuiViewportPtr vp);
public unsafe delegate void Platform_SetWindowPos(ImGuiViewportPtr vp, Vector2 pos);
public unsafe delegate void Platform_SetWindowSize(ImGuiViewportPtr vp, Vector2 size);
public unsafe delegate void Platform_GetWindowSize(ImGuiViewportPtr vp, Vector2* outSize);
public unsafe delegate void Platform_SetWindowFocus(ImGuiViewportPtr vp);
public unsafe delegate byte Platform_GetWindowFocus(ImGuiViewportPtr vp);
public unsafe delegate byte Platform_GetWindowMinimized(ImGuiViewportPtr vp);
public unsafe delegate void Platform_SetWindowTitle(ImGuiViewportPtr vp, IntPtr title);

public class EditorGui {

    public static EditorGui Instance { get; private set; } 
    private readonly List<EditorWindow> _editorWindows = new();
    private readonly EditorDockSpace _editorDockSpace;
    
    private readonly Platform_CreateWindow _createWindow;
    private readonly Platform_DestroyWindow _destroyWindow;
    private readonly Platform_GetWindowPos _getWindowPos;
    private readonly Platform_ShowWindow _showWindow;
    private readonly Platform_SetWindowPos _setWindowPos;
    private readonly Platform_SetWindowSize _setWindowSize;
    private readonly Platform_GetWindowSize _getWindowSize;
    private readonly Platform_SetWindowFocus _setWindowFocus;
    private readonly Platform_GetWindowFocus _getWindowFocus;
    private readonly Platform_GetWindowMinimized _getWindowMinimized;
    private readonly Platform_SetWindowTitle _setWindowTitle;
    
    public unsafe EditorGui(GL gl, Renderer renderer) {
        Instance = this;
        
        ImGuiIOPtr io = ImGui.GetIO();
        io.ConfigFlags = ImGuiConfigFlags.DockingEnable | ImGuiConfigFlags.ViewportsEnable;

//        NullTerminatedString ioBackendPlatformName = io.BackendPlatformName;
//        fixed(byte* bytes = "imgui_impl_glfw\0"u8.ToArray()) {
//            byte* data = ioBackendPlatformName.Data;
//            data = bytes;
//            io.BackendPlatformUserData = (nint) bytes;
//        }
        
        var platformIO = ImGui.GetPlatformIO();
        
        io.BackendFlags |= ImGuiBackendFlags.HasSetMousePos;
        io.BackendFlags |= ImGuiBackendFlags.PlatformHasViewports;
        io.BackendFlags |= ImGuiBackendFlags.RendererHasViewports;
        
//        _createWindow = CreateWindow;
//        _destroyWindow = DestroyWindow;
//        _getWindowPos = GetWindowPos;
//        _showWindow = ShowWindow;
//        _setWindowPos = SetWindowPos;
//        _setWindowSize = SetWindowSize;
//        _getWindowSize = GetWindowSize;
//        _setWindowFocus = SetWindowFocus;
//        _getWindowFocus = GetWindowFocus;
//        _getWindowMinimized = GetWindowMinimized;
//        _setWindowTitle = SetWindowTitle;
//
//        platformIO.Platform_CreateWindow = Marshal.GetFunctionPointerForDelegate(_createWindow);
//        platformIO.Platform_DestroyWindow = Marshal.GetFunctionPointerForDelegate(_destroyWindow);
//        platformIO.Platform_ShowWindow = Marshal.GetFunctionPointerForDelegate(_showWindow);
//        platformIO.Platform_SetWindowPos = Marshal.GetFunctionPointerForDelegate(_setWindowPos);
//        platformIO.Platform_SetWindowSize = Marshal.GetFunctionPointerForDelegate(_setWindowSize);
//        platformIO.Platform_SetWindowFocus = Marshal.GetFunctionPointerForDelegate(_setWindowFocus);
//        platformIO.Platform_GetWindowFocus = Marshal.GetFunctionPointerForDelegate(_getWindowFocus);
//        platformIO.Platform_GetWindowMinimized = Marshal.GetFunctionPointerForDelegate(_getWindowMinimized);
//        platformIO.Platform_SetWindowTitle = Marshal.GetFunctionPointerForDelegate(_setWindowTitle);
//        
////        ImGuiNative.ImGuiPlatformIO_Set_Platform_GetWindowPos(platformIO.NativePtr, Marshal.GetFunctionPointerForDelegate(_getWindowPos));
////        ImGuiNative.ImGuiPlatformIO_Set_Platform_GetWindowSize(platformIO.NativePtr, Marshal.GetFunctionPointerForDelegate(_getWindowSize));
//        
//        platformIO.Platform_GetWindowPos = Marshal.GetFunctionPointerForDelegate(_getWindowPos);
//        platformIO.Platform_GetWindowSize = Marshal.GetFunctionPointerForDelegate(_getWindowSize);
        
        
        io.BackendFlags = ImGuiBackendFlags.HasMouseCursors;
        io.ConfigInputTextCursorBlink = true;
        io.ConfigWindowsResizeFromEdges = true;
        io.ConfigWindowsMoveFromTitleBarOnly = true;
        io.MouseDrawCursor = true;
        
        //ImGui.GetIO().ConfigFlags = ImGuiConfigFlags.ViewportsEnable;
        //ImGui.GetIO().ConfigFlags = ImGuiConfigFlags.NoMouseCursorChange;
        
        //ImGui.GetIO().BackendFlags = ImGuiBackendFlags.HasMouseCursors;
        
        //ImGui.GetIO().ConfigViewportsNoDecoration = true;
        //ImGui.GetIO().ConfigViewportsNoTaskBarIcon = true;
        
        _editorDockSpace = new EditorDockSpace();
        EditorApplication.Instance.EditorLayer.OnDraw += OnDraw;
        
        EditorWindow.Create<HierarchyWindow>();
        AddWindow(new ViewportWindow(gl, renderer));
        EditorWindow.Create<InspectorWindow>();
        EditorWindow.Create<ConsoleWindow>();
        EditorWindow.Create<AssetBrowserWindow>();
        EditorWindow.Create<TerminalWindow>();
    }
    
    private void CreateWindow(ImGuiViewportPtr vp) {
        
    }

    private void DestroyWindow(ImGuiViewportPtr vp) {
        
    }

    private void ShowWindow(ImGuiViewportPtr vp) {
        
    }

    private unsafe void GetWindowPos(ImGuiViewportPtr vp, Vector2* outPos) {
        
    }

    private void SetWindowPos(ImGuiViewportPtr vp, Vector2 pos) {
        
    }

    private void SetWindowSize(ImGuiViewportPtr vp, Vector2 size) {
        
    }

    private unsafe void GetWindowSize(ImGuiViewportPtr vp, Vector2* outSize) {
        
    }

    private void SetWindowFocus(ImGuiViewportPtr vp) {
        
    }

    private byte GetWindowFocus(ImGuiViewportPtr vp) {
        return default;
    }

    private byte GetWindowMinimized(ImGuiViewportPtr vp) {
        return default;
    }

    private unsafe void SetWindowTitle(ImGuiViewportPtr vp, IntPtr title) {

    }
    
    private void OnDraw(Renderer renderer) {
        EditorMainMenubar.Draw(renderer);
        _editorDockSpace.Draw();
        foreach(EditorWindow editorWindow in _editorWindows) {
            editorWindow.DrawWindow();
        }
        
        ImGui.ShowFontSelector("Test");
        
        while(_editorWindowsToBeDestroyed.TryDequeue(out EditorWindow? editorWindow)) {
            if(!_editorWindows.Remove(editorWindow))
                Console.LogWarning("Tried to remove editor window from editor gui but it's not listed");
        }
    }
    
    public void AddWindow(EditorWindow window) {
        if(_editorWindows.Contains(window)) {
            Console.LogWarning("Tried to register editor window to editor gui twice");
            return;
        }
        _editorWindows.Add(window);
    }
    
    private Queue<EditorWindow> _editorWindowsToBeDestroyed = new Queue<EditorWindow>();
    
    public void RemoveWindow(EditorWindow window) {
        _editorWindowsToBeDestroyed.Enqueue(window);
    }
    
    public void EditorUpdate(float deltaTime) {
        foreach(EditorWindow editorWindow in _editorWindows) {
            if(editorWindow is IEditorUpdate updateableEditorWindow) {
                updateableEditorWindow.EditorUpdate(deltaTime);
            }
        }
    }
    
}
