using System.Collections.Generic;
using GameEngine.Entities;

namespace GameEngine; 

public class Scene {

    public string Name { get; set; } = "New Scene";
    public IEnumerable<Entity> Entities => _entities;
    private List<Entity> _entities { get; }

    public Scene() {
        _entities = new List<Entity>();
    }
    
    public Scene(string name, List<Entity> entities) {
        _entities = entities ?? new List<Entity>();
        Name = name;
    }
    
    internal void AddEntity(Entity entity) {
        _entities.Add(entity);
        entity.Awake();
    }
    
}
