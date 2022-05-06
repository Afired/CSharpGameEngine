using System.Collections.Generic;
using GameEngine.Core.Entities;

namespace GameEngine.Core.SceneManagement; 

public class Scene {
    
    public string Name { get; protected set; } = "New Scene";
    public IEnumerable<Entity> Entities => _entities;
    protected List<Entity> _entities { private get; set; } = new();
    
    internal void AddEntity(Entity entity) {
        _entities.Add(entity);
        entity.Awake();
    }
    
}
