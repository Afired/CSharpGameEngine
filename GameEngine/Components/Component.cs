using GameEngine.Entities;

namespace GameEngine.Components;

public class Component {
    
    public Entity Entity { get; }
    
    
    public Component(Entity entity) {
        Entity = entity;
    }
    
}
