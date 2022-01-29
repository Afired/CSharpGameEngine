using System.Collections.Generic;
using GameEngine.Components;

namespace GameEngine.Entities; 

public class Entity {
    
    // a readonly collection of components
    public IReadOnlyList<Component> Components { get; }
    
    protected Entity() {
        Components = new List<Component>();
    }

    internal void Awake() {
        OnAwake();
        foreach(Component component in Components) {
            component.Awake();
        }
    }
    
    internal void Update() {
        OnUpdate();
        foreach(Component component in Components) {
            component.Update();
        }
    }

    internal void PhysicsUpdate() {
        OnPhysicsUpdate();
        foreach(Component component in Components) {
            component.PhysicsUpdate();
        }
    }
    
    protected virtual void OnAwake() { }
    protected virtual void OnUpdate() { }
    protected virtual void OnPhysicsUpdate() { }
    
}
