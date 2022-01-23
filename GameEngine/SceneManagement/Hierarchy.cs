using System.Collections.Generic;
using GameEngine.Entities;

namespace GameEngine.SceneManagement; 

public static class Hierarchy {
    
    public static int Count => _entities.Count;
    public static IEnumerable<Entity> Entities => _entities;
    private static List<Entity> _entities;
    
    
    static Hierarchy() {
        _entities = new List<Entity>();
    }
    
    public static void Add(Entity entity) {
        _entities.Add(entity);
        entity.Awake();
    }
    
}
