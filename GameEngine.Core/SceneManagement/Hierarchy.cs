using System.Collections.Generic;
using GameEngine.Core.Core;
using GameEngine.Core.Entities;
using GameEngine.Core.Physics;

namespace GameEngine.Core.SceneManagement; 

public static class Hierarchy {
    
    //todo: multiple scenes
    
    public static Scene Scene { get; private set; }
    private static Stack<Entity> _entitiesToBeAdded;
    
    static Hierarchy() {
        _entitiesToBeAdded = new Stack<Entity>();
    }
    
    public static void AddEntity(Entity entity) {
        _entitiesToBeAdded.Push(entity);
    }

    public static void LoadScene(Scene scene) {
        PhysicsEngine.InitializeWorld();
        Scene = scene;
        foreach(Entity entity in scene.Entities) {
            entity.Awake();
        }
    }
    
    internal static void Update(float elapsedTime) {
        if(Scene is null)
            return;
        Time.DeltaTime = elapsedTime;
        foreach(Entity entity in Scene.Entities) {
            entity.Update();
        }

        while(_entitiesToBeAdded.TryPop(out Entity entity)) {
            Scene.AddEntity(entity);
            entity.Awake();
        }
    }
    
    internal static void PhysicsUpdate(float physicsTimeStep) {
        if(Scene is null)
            return;
        Time.PhysicsTimeStep = physicsTimeStep;
        foreach(Entity entity in Scene.Entities) {
            entity.PhysicsUpdate();
        }
    }

    internal static void Draw() {
        if(Scene is null)
            return;
        foreach(Entity entity in Scene.Entities) {
            entity.Draw();
        }
    }
    
}
