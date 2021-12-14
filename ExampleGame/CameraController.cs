using GameEngine.Core;
using GameEngine.Debugging;
using GameEngine.Input;
using GameEngine.Numerics;
using GameEngine.Rendering.Cameras;

namespace ExampleGame; 

public class CameraController {

    private BaseCamera _camera;
    private Vector2 _inputAxis;
    private float _speed = 30f;
    
    
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
        
    }

    private void UpdateRotation() {
        Quaternion up = Quaternion.CreateFromAxisAngle(Vector3.Up, Input.MouseDelta.X * 0.005f);
        Quaternion right = Quaternion.CreateFromAxisAngle(Vector3.Right, Input.MouseDelta.Y * 0.005f);

        _camera.Transform.Rotation *= right;
        _camera.Transform.Rotation = up * _camera.Transform.Rotation;
    }
    
}
