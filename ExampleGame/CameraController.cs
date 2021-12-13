using GameEngine.Core;
using GameEngine.Debugging;
using GameEngine.Input;
using GameEngine.Numerics;
using GameEngine.Rendering.Cameras;

namespace ExampleGame; 

public class CameraController {

    private BaseCamera _camera;
    private Vector2 _inputAxis;
    private float _speed = 10f;
    
    
    public CameraController(BaseCamera camera) {
        _camera = camera;
        Game.OnUpdate += OnUpdate;
    }

    private void OnUpdate(float deltaTime) {
        UpdateInputAxis();
        UpdatePosition(deltaTime);
        UpdateRotation();
    }

    private void UpdateInputAxis() {
        _inputAxis = new Vector2();
        _inputAxis.X += Input.IsKeyDown(KeyCode.A) ? -1 : 0;
        _inputAxis.X += Input.IsKeyDown(KeyCode.D) ? 1 : 0;
        _inputAxis.Y += Input.IsKeyDown(KeyCode.S) ? -1 : 0;
        _inputAxis.Y += Input.IsKeyDown(KeyCode.W) ? 1 : 0;
    }

    private void UpdatePosition(float deltaTime) {
        _camera.Transform.Position += _inputAxis.X_Y.Normalized * deltaTime * _speed;
    }

    private void UpdateRotation() {
        _camera.Transform.Rotation += Input.MouseDelta.XY_ * 0.001f;
    }
    
}
