using GameEngine.Core.Core;
using GameEngine.Core.Input;
using GameEngine.Core.Nodes;
using GameEngine.Core.Numerics;
using GameEngine.Core.Serialization;

namespace ExampleGame.Nodes; 

public partial class Player : Transform, IRenderer, IBlaster {
    
    [Serialized] public float Speed { get; init; } = 5f;
    private Vector2 _inputAxis;
    
    protected override void OnUpdate() {
        UpdateInputAxis();
        UpdatePosition();
        UpdateBlaster();
    }
    
    private void UpdateInputAxis() {
        _inputAxis = Vector2.Zero;
        _inputAxis.X += Input.IsKeyDown(KeyCode.A) ? -1 : 0;
        _inputAxis.X += Input.IsKeyDown(KeyCode.D) ? 1 : 0;
        _inputAxis.Y += Input.IsKeyDown(KeyCode.S) ? -1 : 0;
        _inputAxis.Y += Input.IsKeyDown(KeyCode.W) ? 1 : 0;
    }
    
    private void UpdatePosition() {
        LocalPosition += new Vector3(_inputAxis.X, _inputAxis.Y, 0).Normalized * Time.DeltaTime * Speed;
    }
    
    private void UpdateBlaster() {
        Blaster.IsShooting = Input.IsKeyDown(KeyCode.E);
    }
    
}
