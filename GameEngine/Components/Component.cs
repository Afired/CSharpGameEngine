using GameEngine.Entities;

namespace GameEngine.Components;

public abstract class Component {
    
    public Entity Entity { get; }
    
    
    public Component(Entity entity) {
        Entity = entity;
    }

    internal void Awake() => OnAwake();
    internal void Update() => OnUpdate();
    internal void PhysicsUpdate() => OnPhysicsUpdate();
    
    protected virtual void OnAwake() { }
    protected virtual void OnUpdate() { }
    protected virtual void OnPhysicsUpdate() { }
    
}
