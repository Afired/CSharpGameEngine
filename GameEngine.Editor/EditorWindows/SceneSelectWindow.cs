using System.Reflection;
using GameEngine.Core;
using GameEngine.Core.Nodes;
using GameEngine.Core.SceneManagement;
using ImGuiNET;

namespace GameEngine.Editor.EditorWindows; 

public class SceneSelectWindow : EditorWindow {

    private Type[] _sceneTypes;
    
    public SceneSelectWindow() {
        Title = "Scene Select";
        _sceneTypes = GetEnumerableOfType<Scene>().ToArray();
    }
    
    protected override void Draw() {
        
    }
    
    public static IEnumerable<Type> GetEnumerableOfType<T>() where T : class, new() {
        foreach (Type type in Assembly.GetAssembly(typeof(ExampleGame.AssemblyRef))!.GetTypes().Where(myType => myType.IsClass && !myType.IsAbstract && myType.IsSubclassOf(typeof(T)))) {
            yield return type;
        }
    }
    
}
