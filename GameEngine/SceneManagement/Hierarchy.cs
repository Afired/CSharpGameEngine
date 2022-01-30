using System.Collections.Generic;
using GameEngine.Core;
using GameEngine.Entities;
using GameEngine.Physics;

namespace GameEngine.SceneManagement; 

public static class Hierarchy {
    
    //todo: multiple scenes
    
    public static Scene Scene { get; private set; }
    private static Stack<Entity> _toBeAddedEntities;


    static Hierarchy() {
        _toBeAddedEntities = new Stack<Entity>();
    }
    
    public static void AddEntity(Entity entity) {
        if(Scene is null)
            return;
        _toBeAddedEntities.Push(entity);
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
        
        while(_toBeAddedEntities.TryPop(out Entity entity)) {
            Scene.AddEntity(entity);
            entity.Awake();
        }
        
        Time.DeltaTime = elapsedTime;
        foreach(Entity entity in Scene.Entities) {
            entity.Update();
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
