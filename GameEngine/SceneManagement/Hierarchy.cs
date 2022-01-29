using System.Collections.Generic;
using GameEngine.Core;
using GameEngine.Entities;

namespace GameEngine.SceneManagement; 

public static class Hierarchy {
    
    //todo: multiple scenes
    
    public static Scene Scene { get; private set; }
    
    
    static Hierarchy() { }
    
//    public static void AddEntity(Entity entity) {
//        _scene.Entities.Add(entity);
//        entity.Awake();
//    }

    public static void LoadScene(Scene scene) {
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
