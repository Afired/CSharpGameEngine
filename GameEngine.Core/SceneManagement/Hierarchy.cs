using System.Collections.Generic;
using GameEngine.Core.Core;
using GameEngine.Core.Nodes;
using GameEngine.Core.Physics;

namespace GameEngine.Core.SceneManagement; 

public static class Hierarchy {
    
    //todo: multiple scenes
    
    public static Scene? Scene { get; private set; }
    private static Stack<Node> _entitiesToBeAdded;
    
    static Hierarchy() {
        _entitiesToBeAdded = new Stack<Node>();
    }
    
    public static void AddEntity(Node node) {
        _entitiesToBeAdded.Push(node);
    }

    public static void LoadScene(Scene scene) {
        PhysicsEngine.InitializeWorld();
        Scene = scene;
        foreach(Node entity in scene.Entities) {
            entity.Awake();
        }
    }
    
    internal static void Update(float elapsedTime) {
        if(Scene is null)
            return;
        Time.DeltaTime = elapsedTime;
        foreach(Node entity in Scene.Entities) {
            entity.Update();
        }

        while(_entitiesToBeAdded.TryPop(out Node entity)) {
            Scene.AddEntity(entity);
            entity.Awake();
        }
    }
    
    internal static void PhysicsUpdate(float physicsTimeStep) {
        if(Scene is null)
            return;
        Time.PhysicsTimeStep = physicsTimeStep;
        foreach(Node entity in Scene.Entities) {
            entity.PhysicsUpdate();
        }
    }

    internal static void Draw() {
        if(Scene is null)
            return;
        foreach(Node entity in Scene.Entities) {
            entity.Draw();
        }
    }
    
}
