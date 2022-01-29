using ExampleGame.Scenes;
using GameEngine.SceneManagement;
using ImGuiNET;

namespace GameEngine.Editor.EditorWindows; 

public class SceneSelectWindow : EditorWindow {

    public SceneSelectWindow() {
        Title = "Scene Select";
    }
    
    protected override void Draw() {
        if(ImGui.Button("TestScene")) {
            Hierarchy.LoadScene(TestScene.Get());
        }
    }
    
}
