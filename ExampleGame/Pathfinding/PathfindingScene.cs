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
                Transform = { Position = new Vector3(5, 5, 0) }
            },
            new Grid() {
                GridSize = new Size(10, 10),
                NodeRadius = 1f
            }
        };
        
        return new Scene(name, entities);
    }
    
}
