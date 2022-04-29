using System.Collections.Generic;
using ExampleGame.Entities;
using GameEngine;
using GameEngine.Entities;
using GameEngine.Numerics;

namespace ExampleGame.Pathfinding; 

public static class PathfindingScene {
    
    public static Scene Get() {
        
        string name = "Pathfinding Scene";
        
        List<Entity> entities = new() {
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
        
        return new Scene(name, entities);
    }
    
}
