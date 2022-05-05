using System.Collections.Generic;
using GameEngine.Components;
using GameEngine.Entities;

namespace ExampleGame.Pathfinding;

public partial class Node : Entity, IRenderer, ITransform {
    
    private bool _isValid = true;
    public bool IsValid {
        get => _isValid;
        set {
            _isValid = value;
            RefreshTexture();
        }
    }
    
    // distance to starting node
    public int GCost { get; set; }
    // distance to end node
    public int HCost { get; set; }
    // combined cost
    public int FCost => GCost + HCost;
    
    public List<Edge> Edges { get; set; } = new();
    public Node Parent { get; set; }

    protected override void OnAwake() {
        base.OnAwake();
        RefreshTexture();
    }

    private void RefreshTexture() {
        Renderer!.Texture = IsValid ? "Box" : "Checkerboard";
    }

    public void UpdateHCost(Node other) {
        HCost = (int) (Transform.Position - other.Transform.Position).Magnitude;
    }
    
}
