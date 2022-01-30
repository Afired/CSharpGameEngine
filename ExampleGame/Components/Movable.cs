using GameEngine.AutoGenerator;
using GameEngine.Components;
using GameEngine.Core;
using GameEngine.Numerics;

namespace ExampleGame.Components; 

[RequireComponent(typeof(Transform))]
public partial class Movable : Component {

    public Vector3 Direction { get; set; } = Vector3.Zero;
    public float Speed { get; set; } = 10f;
    
    protected override void OnUpdate() {
        UpdatePosition();
    }
    
    private void UpdatePosition() {
        Transform.Position += Direction * Time.DeltaTime * Speed;
    }
    
}
