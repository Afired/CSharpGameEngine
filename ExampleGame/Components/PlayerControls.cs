using GameEngine.Core.AutoGenerator;
using GameEngine.Core.Components;
using GameEngine.Core.Input;
using GameEngine.Core.Numerics;

namespace ExampleGame.Components; 

[RequireComponent(typeof(Movable), typeof(Blaster))]
public partial class PlayerControls : Component {
    
    private Vector2 _inputAxis;
    
    
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
        Movable.Direction = new Vector3(_inputAxis.X, _inputAxis.Y, 0).Normalized;
    }
    
}
