using ExampleGame.Pathfinding;
using ExampleGame.Scenes;
using GameEngine.SceneManagement;
using ImGuiNET;

namespace GameEngine.Editor.EditorWindows; 

public class SceneSelectWindow : EditorWindow {

    public SceneSelectWindow() {
        Title = "Scene Select";
    }
    
    protected override void Draw() {
        if(ImGui.Button("Test Scene")) {
            Hierarchy.LoadScene(TestScene.Get());
        }
        if(ImGui.Button("RigidBody Scene")) {
            Hierarchy.LoadScene(RigidBodyScene.Get());
        }
        if(ImGui.Button("A* Pathfinding Scene")) {
            Hierarchy.LoadScene(PathfindingScene.Get());
        }
    }
    
}
