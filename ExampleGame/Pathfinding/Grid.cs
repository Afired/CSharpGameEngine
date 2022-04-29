using System;
using GameEngine.Components;
using GameEngine.Entities;
using GameEngine.Numerics;
using GameEngine.SceneManagement;

namespace ExampleGame.Pathfinding;

public partial class Grid : Entity, ITransform {

    public static Grid Instance;
    
    public Size GridSize { get; init; }
    public float NodeRadius { get; init; }
    private Node[,] _grid;
    private static Random _random;
    private bool _hasSafeBorder = false;
    
    protected override void OnAwake() {
        _random = new Random(Guid.NewGuid().GetHashCode());
        CreateRandom(_hasSafeBorder);
        LinkNodes();
        Instance = this;
    }
    
    public Node GetRandomBorderNode() {
        if(_grid is null)
            throw new NullReferenceException("Grid has not yet been build!");
        bool isX = _random.Next(0, 2) == 0;
        if(isX) {
            
            bool isOtherSide = _random.Next(0, 2) == 0;
            int y = isOtherSide ? GridSize.Y - 1 : 0;
            
            int xIndex = _random.Next(0, GridSize.X);
            
            _grid[xIndex, y].IsValid = true;
            
            return _grid[xIndex, y];
        } else {
            
            bool isOtherSide = _random.Next(0, 2) == 0;
            int x = isOtherSide ? GridSize.X - 1 : 0;
            
            int yIndex = _random.Next(0, GridSize.Y);
            
            _grid[x, yIndex].IsValid = true;
            
            return _grid[x, yIndex];
        }
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
    
    private void CreateRandom(bool hasSafeBorder) {
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
        
        if(!hasSafeBorder)
            return;
        
        //make sure there is a possible solution by creating a safe border
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
