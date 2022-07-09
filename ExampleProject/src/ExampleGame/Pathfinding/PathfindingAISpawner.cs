/*using System;
using GameEngine.Core.Core;
using GameEngine.Core.Entities;
using GameEngine.Core.Input;
using GameEngine.Core.SceneManagement;

namespace ExampleGame.Pathfinding; 

public partial class PathfindingAISpawner : GameEngine.Core.Entities.Node {
    
    private float _cooldown = 1f;
    private float _currentTime;
    
    protected override void OnUpdate() {
        _currentTime -= Time.DeltaTime;
        if(_currentTime > 0)
            return;
        if(Input.IsKeyDown(KeyCode.E)) {
            SpawnPathfindingAI();
            _currentTime = _cooldown;
        }
    }

    private void SpawnPathfindingAI() {

        Grid grid = Grid.Instance;

        Random random = new();

        PNode startNode = grid.GetRandomBorderNode();
        PNode endNode = grid.GetRandomBorderNode();
        
        PathfindingAI pathfindingAi = new() {
            StartNode = startNode,
            EndNode = endNode,
        };
        Hierarchy.AddEntity(pathfindingAi);
        startNode.Renderer.Shader = "";
        endNode.Renderer.Shader = "";
    }
    
}
*/