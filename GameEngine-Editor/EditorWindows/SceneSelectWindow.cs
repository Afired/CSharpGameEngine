using System.Reflection;
using ExampleGame.Scenes;
using GameEngine.SceneManagement;
using ImGuiNET;

namespace GameEngine.Editor.EditorWindows; 

public class SceneSelectWindow : EditorWindow {

    private Type[] _sceneTypes;
    
    public SceneSelectWindow() {
        Title = "Scene Select";
        _sceneTypes = GetEnumerableOfType<Scene>().ToArray();
    }
    
    protected override void Draw() {
//        if(ImGui.Button("Test Scene")) {
//            Hierarchy.LoadScene(new TestScene());
//        }
//        if(ImGui.Button("RigidBody Scene")) {
//            Hierarchy.LoadScene(new RigidBodyScene());
//        }
//        if(ImGui.Button("A* Pathfinding Scene")) {
//            Hierarchy.LoadScene(new PathfindingScene());
//        }

        foreach(Type sceneType in _sceneTypes) {
            if(ImGui.Button(sceneType.Name)) {
                Hierarchy.LoadScene(Activator.CreateInstance(sceneType) as Scene);
            }
        }
    }
    
    public static IEnumerable<Type> GetEnumerableOfType<T>() where T : class, new() {
        foreach (Type type in Assembly.GetAssembly(typeof(ExampleGame.AssemblyRef))!.GetTypes().Where(myType => myType.IsClass && !myType.IsAbstract && myType.IsSubclassOf(typeof(T)))) {
            yield return type;
        }
    }
    
}
