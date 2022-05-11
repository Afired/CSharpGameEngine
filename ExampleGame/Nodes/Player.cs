using GameEngine.Core.Core;
using GameEngine.Core.Input;
using GameEngine.Core.Nodes;
using GameEngine.Core.Numerics;

namespace ExampleGame.Nodes; 

public partial class Player : Transform, IRenderer, IBlaster {
    
    private Vector2 _inputAxis;
    public float Speed { get; init; }= 5f;
    
    protected override void OnUpdate() {
        UpdateInputAxis();
        UpdateMovable();
        UpdateBlaster();
    }
    
    private void UpdateBlaster() {
        Blaster.IsShooting = Input.IsKeyDown(KeyCode.E);
    }
    
    private void UpdateInputAxis() {
        _inputAxis = Vector2.Zero;
        _inputAxis.X += Input.IsKeyDown(KeyCode.A) ? -1 : 0;
        _inputAxis.X += Input.IsKeyDown(KeyCode.D) ? 1 : 0;
        _inputAxis.Y += Input.IsKeyDown(KeyCode.S) ? -1 : 0;
        _inputAxis.Y += Input.IsKeyDown(KeyCode.W) ? 1 : 0;
    }
    
    private void UpdateMovable() {
        LocalPosition += new Vector3(_inputAxis.X, _inputAxis.Y, 0).Normalized * Time.DeltaTime * Speed;
    }
    
}
