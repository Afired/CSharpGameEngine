using System;
using GameEngine.Core.Components;
using GameEngine.Core.Entities;
using GameEngine.Core.Numerics;
using GameEngine.Core.SceneManagement;

namespace ExampleGame.Pathfinding;

public partial class Grid : Entity, ITransform {
    
    public static Grid Instance;
    
    public Size GridSize { get; init; } = new Size(20, 20);
    public uint ValidNodeProbability { get; init; } = 0;
    public bool HasSafeBorder { get; init; } = true;
    public bool ConnectNodesDiagonal { get; init; } = true;
    public bool ConnectNodesStraight { get; init; } = true;
    public int StraightConnectionCost { get; init; } = 10;
    public int DiagonalConnectionCost { get; init; } = 14; // 14: prefers diagonal | 30: prefers straight over diagonal if its not too complicated
    public float NodeSpacing { get; init; } = 1f;
    private Node[,] _grid;
    private static Random _random;
    
    protected override void OnAwake() {
        _random = new Random(Guid.NewGuid().GetHashCode());
        CreateRandom(HasSafeBorder);
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

                if(ConnectNodesStraight) {
                    
                    if(x > 0)
                        _grid[x, y].Edges.Add(new Edge(_grid[x - 1, y], StraightConnectionCost));
                    if(x < GridSize.X - 1)
                        _grid[x, y].Edges.Add(new Edge(_grid[x + 1, y], StraightConnectionCost));
                
                    if(y > 0)
                        _grid[x, y].Edges.Add(new Edge(_grid[x, y - 1], StraightConnectionCost));
                    if(y < GridSize.Y - 1)
                        _grid[x, y].Edges.Add(new Edge(_grid[x, y + 1], StraightConnectionCost));
                    
                }

                if(ConnectNodesDiagonal) {
                    
                    if(x > 0 && y > 0)
                        _grid[x, y].Edges.Add(new Edge(_grid[x - 1, y - 1], DiagonalConnectionCost));
                    if(x > 0 && y < GridSize.Y - 1)
                        _grid[x, y].Edges.Add(new Edge(_grid[x - 1, y + 1], DiagonalConnectionCost));
                    if(x < GridSize.X - 1 && y > 0)
                        _grid[x, y].Edges.Add(new Edge(_grid[x + 1, y - 1], DiagonalConnectionCost));
                    if(x < GridSize.X - 1 && y < GridSize.Y - 1)
                        _grid[x, y].Edges.Add(new Edge(_grid[x + 1, y + 1], DiagonalConnectionCost));
                    
                }
                
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
                    Transform = { Position = Transform.Position + new Vector3(x * NodeSpacing, y * NodeSpacing, 0) },
                    IsValid = _random.Next(0, (int) ValidNodeProbability + 1) != 0,
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
