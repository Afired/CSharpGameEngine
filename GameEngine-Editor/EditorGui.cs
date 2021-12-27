using GameEngine.Editor.EditorWindows;

namespace GameEngine.Editor; 

public class EditorGui {
    
    public EditorGui() {
        Initialize();
    }
    
    private void Initialize() {
        new EditorDockSpace();
        new HierarchyWindow();
        new ViewportWindow();
        new InspectorWindow();
        new ConsoleWindow();
        new AssetBrowserWindow();
        new ProjectSettingsWindow();
    }
    
}
