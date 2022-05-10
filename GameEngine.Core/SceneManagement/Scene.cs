using System.Collections.Generic;
using GameEngine.Core.Entities;

namespace GameEngine.Core.SceneManagement; 

public class Scene {
    
    public string Name { get; protected set; } = "New Scene";
    public IEnumerable<Node> Entities => _entities;
    protected List<Node> _entities { private get; set; } = new();
    
    internal void AddEntity(Node node) {
        _entities.Add(node);
        node.Awake();
    }
    
}
