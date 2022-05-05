using GameEngine.Core.AutoGenerator;
using GameEngine.Core.Components;
using GameEngine.Core.Core;
using GameEngine.Core.Numerics;

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
