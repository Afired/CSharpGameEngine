using System.Collections.Generic;
using ExampleGame.Entities;
using GameEngine;
using GameEngine.Entities;
using GameEngine.Numerics;

namespace ExampleGame.Pathfinding; 

public class PathfindingScene : Scene {
    
    public PathfindingScene() {
        Name = "Pathfinding Scene";
        
        // small grid, straight only
        _entities = new List<Entity>() {
            
            new DynamicCamera() {
                Transform = { Position = new Vector3(10, 10, 0) },
                Camera2D = { Zoom = 40 }
            },
            new PathfindingAISpawner(),
            new Grid() {
                GridSize = new Size(20, 20),
                ValidNodeProbability = 3,
                HasSafeBorder = false,
                ConnectNodesStraight = true,
                ConnectNodesDiagonal = false,
                StraightConnectionCost = 10,
                DiagonalConnectionCost = 14,
                NodeSpacing = 1f
            }
            
        };
        
        // small grid, straight and diagonal balanced
//        _entities = new List<Entity>() {
//            
//            new DynamicCamera() {
//                Transform = { Position = new Vector3(10, 10, 0) },
//                Camera2D = { Zoom = 40 }
//            },
//            new PathfindingAISpawner(),
//            new Grid() {
//                GridSize = new Size(20, 20),
//                ValidNodeProbability = 2,
//                HasSafeBorder = false,
//                ConnectNodesStraight = true,
//                ConnectNodesDiagonal = true,
//                StraightConnectionCost = 10,
//                DiagonalConnectionCost = 14,
//                NodeSpacing = 1f
//            }
//            
//        };
        
        // small grid, diagonal only
//        _entities = new List<Entity>() {
//            
//            new DynamicCamera() {
//                Transform = { Position = new Vector3(10, 10, 0) },
//                Camera2D = { Zoom = 40 }
//            },
//            new PathfindingAISpawner(),
//            new Grid() {
//                GridSize = new Size(20, 20),
//                ValidNodeProbability = 2,
//                HasSafeBorder = false,
//                ConnectNodesStraight = false,
//                ConnectNodesDiagonal = true,
//                StraightConnectionCost = 10,
//                DiagonalConnectionCost = 14,
//                NodeSpacing = 1f
//            }
//            
//        };
        
        // small grid, pref straight
//        _entities = new List<Entity>() {
//            
//            new DynamicCamera() {
//                Transform = { Position = new Vector3(10, 10, 0) },
//                Camera2D = { Zoom = 40 }
//            },
//            new PathfindingAISpawner(),
//            new Grid() {
//                GridSize = new Size(20, 20),
//                ValidNodeProbability = 2,
//                HasSafeBorder = false,
//                ConnectNodesStraight = true,
//                ConnectNodesDiagonal = true,
//                StraightConnectionCost = 10,
//                DiagonalConnectionCost = 30,
//                NodeSpacing = 1f
//            }
//            
//        };
        
        // big grid, straight and diagonal balanced
//        _entities = new List<Entity>() {
//            
//            new DynamicCamera() {
//                Transform = { Position = new Vector3(40, 40, 0) },
//                Camera2D = { Zoom = 10 }
//            },
//            new PathfindingAISpawner(),
//            new Grid() {
//                GridSize = new Size(80, 80),
//                ValidNodeProbability = 1,
//                HasSafeBorder = true,
//                ConnectNodesStraight = true,
//                ConnectNodesDiagonal = true,
//                StraightConnectionCost = 10,
//                DiagonalConnectionCost = 14,
//                NodeSpacing = 1f
//            }
//            
//        };
        
    }
    
}
