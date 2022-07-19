using GameEngine.Core.SceneManagement;
using ImGuiNET;

namespace GameEngine.Editor.EditorWindows; 

public class AssetBrowserWindow : EditorWindow {
    
    public string Selected { get; private set; } = string.Empty;
    private const string ASSETS_DIRECTORY_PATH = @"D:\Dev\C#\CSharpGameEngine\ExampleProject\Assets";
    
    public AssetBrowserWindow() {
        Title = "AssetBrowser";
    }
    
    protected override void Draw() {
        
        if(ImGui.BeginMenuBar()) {
            ImGui.Text(ASSETS_DIRECTORY_PATH);
            ImGui.EndMenuBar();
        }
        
        DrawFolder(ASSETS_DIRECTORY_PATH);
        
        if(ImGui.IsMouseDown(ImGuiMouseButton.Left) && ImGui.IsWindowHovered()) {
            Selection.Clear();
        }
        
    }
    
    private void DrawFile(string currentPath) {
        ImGuiTreeNodeFlags treeNodeFlags = ImGuiTreeNodeFlags.Bullet |
                                           ImGuiTreeNodeFlags.NoTreePushOnOpen |
                                           (Selected == currentPath ? ImGuiTreeNodeFlags.Selected : ImGuiTreeNodeFlags.None) |
                                           ImGuiTreeNodeFlags.SpanFullWidth;
        ImGui.PushID(currentPath.GetHashCode());
        ImGui.TreeNodeEx(Path.GetFileName(currentPath), treeNodeFlags);
        ImGui.PopID();
        if(ImGui.IsItemClicked()) {
            Selected = currentPath;
        }
        
        if(ImGui.BeginPopupContextItem()) {
            if(ImGui.MenuItem("Delete File"))
                Console.LogWarning("Deleting is not implemented yet");
            ImGui.EndPopup();
        }
    }
    
    private void DrawFolder(string currentPath) {
        ImGuiTreeNodeFlags treeNodeFlags = ImGuiTreeNodeFlags.OpenOnArrow |
                                           (Selected == currentPath ? ImGuiTreeNodeFlags.Selected : ImGuiTreeNodeFlags.None) |
                                           ImGuiTreeNodeFlags.SpanFullWidth;
        ImGui.PushID(currentPath.GetHashCode());
        bool opened = ImGui.TreeNodeEx(Path.GetFileName(currentPath), treeNodeFlags);
        ImGui.PopID();
        if(ImGui.IsItemClicked()) {
            Selected = currentPath;
        }
        
        if(ImGui.BeginPopupContextItem()) {
            if(ImGui.MenuItem("Delete Folder"))
                Console.LogWarning("Deleting is not implemented yet");
            ImGui.EndPopup();
        }
        
        if(opened) {

            foreach(string directoryPath in Directory.EnumerateDirectories(currentPath)) {
                DrawFolder(directoryPath);
            }
            
            foreach(string filePath in Directory.EnumerateFiles(currentPath)) {
                DrawFile(filePath);
            }
            
            ImGui.TreePop();
        }
    }
    
}
