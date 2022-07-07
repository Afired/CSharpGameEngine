using GameEngine.Core.Nodes;
using GameEngine.Core.SceneManagement;
using GameEngine.Core.Serialization;

namespace GameEngine.Editor; 

public static class PlayMode {
    
    public static Mode Current { get; private set; }
    
    public static void Start() {
        if(Current != Mode.Editing)
            throw new Exception();
        // SceneSerializer.SaveOpenedScene();
        Hierarchy.SaveCurrentRootNode();
        Current = Mode.Playing;
    }
    
    public static void Pause() {
        if(Current != Mode.Playing)
            throw new Exception();
        Current = Mode.Paused;
    }
    
    public static void Resume() {
        if(Current != Mode.Paused)
            throw new Exception();
        Current = Mode.Playing;
    }
    
    public static void Stop() {
        if(Current != Mode.Playing && Current != Mode.Paused)
            throw new Exception();
        Current = Mode.Editing;
        Hierarchy.SetRootNode(Serializer.Deserialize<Node>("Test"));
    }
    
    public enum Mode {
        Editing,
        Playing,
        Paused,
    }
    
}
