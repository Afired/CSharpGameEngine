using System.Collections.Generic;
using GameEngine.Core.Nodes;
using GameEngine.Core.Serialization;

namespace GameEngine.Core.SceneManagement; 

public class Scene {
    
    [Serialized] public string Name { get; protected set; } = "New Scene";
    public IEnumerable<Node> Entities => _entities;
//    [Serialized] protected List<Node> _entities { private get; set; } = new();
    [Serialized] public List<Node> _entities { private get; set; } = new();
    
    internal void AddEntity(Node node) {
        _entities.Add(node);
        node.Awake();
    }
    
}
