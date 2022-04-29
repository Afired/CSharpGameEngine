using System.Collections.Generic;
using GameEngine.Components;
using GameEngine.Core;
using GameEngine.Entities;
using GameEngine.Numerics;

namespace ExampleGame.Pathfinding; 

public partial class PathfindingAI : Entity, ITransform, IRenderer {
    
    public Node StartNode { get; set; }
    public Node EndNode { get; set; }

    private List<Node> _nodePath;
    
    protected override void OnAwake() {
        base.OnAwake();

        Renderer.Shader = "";
        Transform.Position = new Vector3(StartNode.Transform.Position.X, StartNode.Transform.Position.Y, 1);
        _nodePath = AStar.Search(StartNode, EndNode) ?? new();
    }
    
    private float _currentTime = 1f;
    private float _delay = 0.25f;
    
    protected override void OnUpdate() {
        base.OnUpdate();
        
        if(_nodePath.Count == 0)
            return;
        
        _currentTime -= Time.DeltaTime;
        if(_currentTime < 0) {
            _currentTime = _delay;

            Vector3 newPosition = _nodePath[0].Transform.Position;
            Transform.Position = new Vector3(newPosition.X, newPosition.Y, 1);
            _nodePath.RemoveAt(0);
        }
    }
    
}
