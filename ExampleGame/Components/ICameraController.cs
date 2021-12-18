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
        UpdatePosition();
    }

    private void UpdateInputAxis() {
        _inputAxis = new Vector2();
        _inputAxis.X += Input.IsKeyDown(KeyCode.A) ? -1 : 0;
        _inputAxis.X += Input.IsKeyDown(KeyCode.D) ? 1 : 0;
        _inputAxis.Y += Input.IsKeyDown(KeyCode.S) ? -1 : 0;
        _inputAxis.Y += Input.IsKeyDown(KeyCode.W) ? 1 : 0;
    }

    private void UpdatePosition() {
        if(Input.IsKeyDown(KeyCode.F)) {
            (GameObject as ITransform).Transform.Position += -Input.MouseDelta.XY_ * 0.00005f;
        }
    }
    
}

public interface ICameraController : ITransform {
    CameraController CameraController { get; set; }
}