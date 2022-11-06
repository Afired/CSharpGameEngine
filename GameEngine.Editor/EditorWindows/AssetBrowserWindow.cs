using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;
using GameEngine.Core;
using GameEngine.Core.AssetManagement;
using GameEngine.Core.Nodes;
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
        if(ImGui.BeginDragDropSource()) {
            unsafe {
                Guid guid = AssetManager.Instance.GetGuidOfAsset(filePath);
                IntPtr guidPtr = Marshal.AllocHGlobal(sizeof(Guid));
                Marshal.StructureToPtr(guid, guidPtr, false);
                ImGui.SetDragDropPayload(typeof(Guid).FullName, guidPtr, (uint) sizeof(Guid));
                ImGui.EndDragDropSource();
                Marshal.FreeHGlobal(guidPtr);
            }
        }
        ImGui.PopID();
        
        if(ImGui.IsItemClicked(ImGuiMouseButton.Right) || ImGui.IsItemClicked(ImGuiMouseButton.Left)) {
            Selected = filePath;
        }
        
        if(ImGui.IsItemHovered() && ImGui.IsMouseDoubleClicked(ImGuiMouseButton.Left)) {
            if(Path.GetExtension(filePath) == ".node") {
//                Hierarchy.SetRootNode(Serializer.DeserializeNode(filePath));
//                Hierarchy.CurrentlyLoadedNodesAssetPath = filePath;
                Hierarchy.Open(new AssetRef<Node>(AssetManager.Instance.GetGuidOfAsset(filePath)));
            }
        }
        
        if(ImGui.BeginPopupContextItem()) {
            if(ImGui.MenuItem("Open in Explorer"))
                Process.Start("explorer.exe" , Path.GetDirectoryName(filePath));
            if(ImGui.MenuItem("Open File"))
                Process.Start("explorer.exe" , filePath);
            if(ImGui.MenuItem("Delete File"))
                Console.LogWarning("Deleting is not implemented yet");
            if(ImGui.MenuItem("Reimport")) {
//                Guid guid = AssetManager.Instance.GetGuidOfAsset(filePath);
//                AssetDatabase.Unload(guid);
//                AssetImport.Import(filePath);
//                AssetDatabase.Load(guid, IAssetImporter.Import);
                Console.LogWarning("Reimporting is not implemented yet");
            }
            ImGui.EndPopup();
        }
    }
    
    private void DrawFolder(string currentPath) {
        ImGuiTreeNodeFlags treeNodeFlags = ImGuiTreeNodeFlags.OpenOnArrow |
                                           (Selected == currentPath ? ImGuiTreeNodeFlags.Selected : ImGuiTreeNodeFlags.None) |
                                           ImGuiTreeNodeFlags.SpanFullWidth | ImGuiTreeNodeFlags.OpenOnDoubleClick;
        ImGui.PushID(currentPath.GetHashCode());
        bool opened = ImGui.TreeNodeEx(Path.GetFileName(currentPath), treeNodeFlags);
        ImGui.PopID();
        if(ImGui.IsItemClicked(ImGuiMouseButton.Right) || ImGui.IsItemClicked(ImGuiMouseButton.Left)) {
            Selected = currentPath;
        }
        
        if(ImGui.BeginPopupContextItem(currentPath, ImGuiPopupFlags.MouseButtonRight)) {
            
            if(ImGui.MenuItem("Open in Explorer"))
                Process.Start("explorer.exe" , currentPath);
            
            if(ImGui.BeginMenu("Create Node")) {
                foreach(Assembly assembly in EditorApplication.Instance.AssemblyLoadContextManager.ExternalAssemblies.Append(typeof(Application).Assembly)) {
                    foreach(Type type in assembly.GetTypes().Where(type => !type.IsAbstract && type.IsAssignableTo(typeof(Node)))) {
                        if(ImGui.MenuItem(type.Name)) {
                            Node newNode = Node.New(type);
                            string nodeName = $"New {type.Name}";
                            while(File.Exists($@"{currentPath}\{nodeName}.node")) {
                                nodeName += "_";
                            }
                            File.WriteAllText($@"{currentPath}\{nodeName}.node", Serializer.SerializeNode(newNode));
                        }
                    }
                }
                
                ImGui.EndMenu();
            }
            
            ImGui.EndPopup();
        }
        
        if(opened) {

            foreach(string directoryPath in Directory.EnumerateDirectories(currentPath)) {
                DrawFolder(directoryPath);
            }
            
            foreach(string filePath in Directory.EnumerateFiles(currentPath).Where(filePath => Path.GetExtension(filePath) != ".meta")) {
                DrawFile(filePath);
            }
            
            ImGui.TreePop();
        }
    }
    
}
