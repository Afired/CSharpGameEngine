using System;
using GameEngine.Components;
using GameEngine.Entities;
using GameEngine.Numerics;
using GameEngine.SceneManagement;

namespace ExampleGame.Pathfinding;

public partial class Grid : Entity, ITransform {
    
    public Size GridSize { get; init; }
    public float NodeRadius { get; init; }
    private Node[,] _grid;
    private static Random _random;
    
    protected override void OnAwake() {
        _random = new Random();
        CreateRandom();
        LinkNodes();
    }

    private void LinkNodes() {
        for(int x = 0; x < GridSize.X; x++) {
            for(int y = 0; y < GridSize.Y; y++) {
                
                if(x > 0)
                    _grid[x, y].Neighbors.Add(_grid[x - 1, y]);
                if(x < GridSize.X - 1)
                    _grid[x, y].Neighbors.Add(_grid[x + 1, y]);
                
                if(y > 0)
                    _grid[x, y].Neighbors.Add(_grid[x, y - 1]);
                if(y < GridSize.Y - 1)
                    _grid[x, y].Neighbors.Add(_grid[x, y + 1]);
                
            }
        }
    }
    
    private void CreateRandom() {

        if(GridSize.X < 2 || GridSize.Y < 2)
            throw new Exception("GridSize should at least be 2 units!");
        
        _grid = new Node[GridSize.X, GridSize.Y];
        for(int x = 0; x < GridSize.X; x++) {
            for(int y = 0; y < GridSize.Y; y++) {
                Node newNode = new() {
                    Transform = { Position = Transform.Position + new Vector3(x * NodeRadius, y * NodeRadius, 0) },
                    IsValid = _random.Next(0, 4) != 0,
                };
                Hierarchy.AddEntity(newNode);
                _grid[x, y] = newNode;
            }
        }
        
        //make sure there is a possible solution
        for(int x = 0; x < GridSize.X; x++) {
            _grid[0, x].IsValid = true;
            _grid[GridSize.Y - 1, x].IsValid = true;
        }
        for(int y = 0; y < GridSize.Y; y++) {
            _grid[y, 0].IsValid = true;
            _grid[y, GridSize.X - 1].IsValid = true;
        }
    }
    
}

public struct Size {
    public int X { get; set; }
    public int Y { get; set; }

    public Size(int x, int y) {
        X = x;
        Y = y;
    }
}
