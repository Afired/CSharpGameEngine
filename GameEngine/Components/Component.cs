using GameEngine.Entities;

namespace GameEngine.Components;

public abstract class Component {
    
    public Entity Entity { get; }
    
    
    public Component(Entity entity) {
        Entity = entity;
        Init();
    }
    
    /// <summary>
    /// init callback should be used for self initialization only
    /// </summary>
    protected virtual void Init() { }
    
}
