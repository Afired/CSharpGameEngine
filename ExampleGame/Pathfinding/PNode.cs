using System.Collections.Generic;
using GameEngine.Core.Components;
using GameEngine.Core.Entities;

namespace ExampleGame.Pathfinding;

public partial class PNode : GameEngine.Core.Entities.Node, IRenderer, ITransform {
    
    private bool _isValid = true;
    public bool IsValid {
        get => _isValid;
        set {
            _isValid = value;
            RefreshTexture();
        }
    }
    
    // cost from starting node
    public int GCost { get; set; }
    // distance to end node
    public int HCost { get; set; }
    // combined cost
    public int FCost => GCost + HCost;
    
    public List<Edge> Edges { get; set; } = new();
    public PNode Parent { get; set; }

    protected override void OnAwake() {
        base.OnAwake();
        RefreshTexture();
    }

    private void RefreshTexture() {
        Renderer!.Texture = IsValid ? "Box" : "Checkerboard";
    }

    public void UpdateHCost(PNode other) {
        HCost = (int) (Transform.Position - other.Transform.Position).Magnitude;
    }
    
}
