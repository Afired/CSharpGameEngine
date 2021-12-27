using GameEngine.Editor.EditorWindows;

namespace GameEngine.Editor; 

public class EditorGui {
    
    public EditorGui() {
        Initialize();
    }
    
    private void Initialize() {
        new EditorDockSpace();
        new EditorWindow();
    }
    
}
