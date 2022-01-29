using System.Collections.Generic;
using GameEngine.Entities;

namespace GameEngine; 

public class Scene {
    
    public string Name { get; set; }
    public IEnumerable<Entity> Entities => _entities;
    private List<Entity> _entities { get; }

    public Scene() {
        _entities = new List<Entity>();
    }
    
}
