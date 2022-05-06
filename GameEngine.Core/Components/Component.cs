using GameEngine.Core.Entities;

namespace GameEngine.Core.Components;

public abstract class Component {
    
    public Entity Entity { get; }
    
    
    public Component(Entity entity) {
        Entity = entity;
    }

    internal void Awake() => OnAwake();
    internal void Update() => OnUpdate();
    internal void PhysicsUpdate() => OnPhysicsUpdate();
    internal void Draw() => OnDraw();
    
    protected virtual void OnAwake() { }
    protected virtual void OnUpdate() { }
    protected virtual void OnPhysicsUpdate() { }
    protected virtual void OnDraw() { }
    
}
