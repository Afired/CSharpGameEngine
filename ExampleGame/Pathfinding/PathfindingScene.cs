using System.Collections.Generic;
using ExampleGame.Entities;
using GameEngine;
using GameEngine.Entities;
using GameEngine.Numerics;

namespace ExampleGame.Pathfinding; 

public class PathfindingScene : Scene {
    
    public PathfindingScene() {
        Name = "Pathfinding Scene";
        
        _entities = new List<Entity>() {
            
            new DynamicCamera() {
                Transform = { Position = new Vector3(10, 10, 0) },
                Camera2D = { Zoom = 40 }
            },
            new PathfindingAISpawner(),
            new Grid() {
                GridSize = new Size(20, 20),
                NodeRadius = 1f
            }
            
        };
    }
    
}
