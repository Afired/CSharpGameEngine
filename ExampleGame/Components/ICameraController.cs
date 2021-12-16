using GameEngine.Components;
using GameEngine.Core;
using GameEngine.Input;
using GameEngine.Numerics;

namespace ExampleGame; 

public class CameraController : Component {
    
    private Vector2 _inputAxis;
    private float _speed = 30f;

    private Transform Transform => (GameObject as ITransform).Transform;
    
    
    public CameraController(GameObject gameObject) : base(gameObject) {
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

        Transform.Rotation *= right;
        Transform.Rotation = up * Transform.Rotation;
    }
    
}

public interface ICameraController {
    CameraController CameraController { get; set; }
}