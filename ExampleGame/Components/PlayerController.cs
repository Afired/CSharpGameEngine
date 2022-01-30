using GameEngine.AutoGenerator;
using GameEngine.Components;
using GameEngine.Core;
using GameEngine.Input;
using GameEngine.Numerics;

namespace ExampleGame.Components; 

[RequireComponent(typeof(Transform))]
public partial class PlayerController : Component {
    
    private Vector2 _inputAxis;
    public float Speed { get; set; } = 10f;
    
    
    protected override void OnUpdate() {
        UpdateInputAxis();
        UpdatePosition();
    }
    
    private void UpdateInputAxis() {
        _inputAxis = Vector2.Zero;
        _inputAxis.X += Input.IsKeyDown(KeyCode.A) ? -1 : 0;
        _inputAxis.X += Input.IsKeyDown(KeyCode.D) ? 1 : 0;
        _inputAxis.Y += Input.IsKeyDown(KeyCode.S) ? -1 : 0;
        _inputAxis.Y += Input.IsKeyDown(KeyCode.W) ? 1 : 0;
    }
    
    private void UpdatePosition() {
        Transform.Position += _inputAxis.XY_.Normalized * Time.DeltaTime * Speed;
    }
    
}
