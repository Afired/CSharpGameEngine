using GameEngine.Core.Rendering;
using GameEngine.Editor.EditorWindows;
using ImGuiNET;

namespace GameEngine.Editor; 

public class EditorGui {

    public static EditorGui Instance { get; private set; } 
    private readonly List<EditorWindow> _editorWindows = new();
    private readonly EditorDockSpace _editorDockSpace;
    
    public EditorGui() {
        Instance = this;
        
        ImGuiIOPtr io = ImGui.GetIO();
        io.ConfigFlags = ImGuiConfigFlags.DockingEnable;
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
        EditorWindow.Create<ViewportWindow>();
        EditorWindow.Create<InspectorWindow>();
        EditorWindow.Create<ConsoleWindow>();
        EditorWindow.Create<AssetBrowserWindow>();
        EditorWindow.Create<TerminalWindow>();
    }
    
    private void OnDraw(Renderer renderer) {
        EditorMainMenubar.Draw(renderer);
        _editorDockSpace.Draw();
        foreach(EditorWindow editorWindow in _editorWindows) {
            editorWindow.DrawWindow();
        }
    }
    
    public void AddWindow(EditorWindow window) {
        if(_editorWindows.Contains(window)) {
            Console.LogWarning("Tried to register editor window to editor gui twice");
            return;
        }
        _editorWindows.Add(window);
    }
    
    public void RemoveWindow(EditorWindow window) {
        if(!_editorWindows.Remove(window))
            Console.LogWarning("Tried to remove editor window from editor gui but it's not listed");
    }
    
}
