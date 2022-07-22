using GameEngine.Core.SceneManagement;
using GameEngine.Core.Serialization;
using ImGuiNET;

namespace GameEngine.Editor.EditorWindows; 

public class AssetBrowserWindow : EditorWindow {
    
    public string Selected { get; private set; } = string.Empty;
    
    public AssetBrowserWindow() {
        Title = "AssetBrowser";
    }
    
    protected override void Draw() {
        
        if(ImGui.BeginMenuBar()) {
            ImGui.Text(Project.Current?.ProjectAssetDirectory ?? string.Empty);
            ImGui.EndMenuBar();
        }
        
        if(Project.Current is not null)
            DrawFolder(Project.Current.ProjectAssetDirectory!);
        
        if(ImGui.IsMouseDown(ImGuiMouseButton.Left) && ImGui.IsWindowHovered()) {
            Selection.Clear();
        }
        
    }
    
    private void DrawFile(string filePath) {
        ImGuiTreeNodeFlags treeNodeFlags = ImGuiTreeNodeFlags.Bullet |
                                           ImGuiTreeNodeFlags.NoTreePushOnOpen |
                                           (Selected == filePath ? ImGuiTreeNodeFlags.Selected : ImGuiTreeNodeFlags.None) |
                                           ImGuiTreeNodeFlags.SpanFullWidth;
        ImGui.PushID(filePath.GetHashCode());
        ImGui.TreeNodeEx(Path.GetFileName(filePath), treeNodeFlags);
        ImGui.PopID();
        if(ImGui.IsItemClicked()) {
            Selected = filePath;
        }
        
        if(ImGui.IsItemHovered() && ImGui.IsMouseDoubleClicked(ImGuiMouseButton.Left)) {
            Console.Log($"double clicked: {filePath}");
            Console.Log(Path.GetExtension(filePath));
            if(Path.GetExtension(filePath) == ".node") {
                Hierarchy.SetRootNode(Serializer.DeserializeNode(filePath));
            }
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
