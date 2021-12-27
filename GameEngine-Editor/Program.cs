using GameEngine.Core;

namespace GameEngine.Editor;

public static class Program {

    public static int Main(string[] args) {

        Application application = new Application();
        
        application.Initialize();

        Configuration.WindowTitle = "GameEngine-Editor";
        
        EditorGui editorGui = new EditorGui();
        
        application.Start();
        
        return 0;
    }
    
}
