using GameEngine.Components;
using GameEngine.Core;
using GameEngine.Input;

namespace ExampleGame.Components; 

public class CameraController : Component {
    
    private float _speed = 0.005f;
    
    
    public CameraController(GameObject gameObject) : base(gameObject) {
        Game.OnUpdate += OnUpdate;
    }
    
    private void OnUpdate(float deltaTime) {
        UpdatePosition();
    }
    
    private void UpdatePosition() {
        if(Input.IsKeyDown(KeyCode.LeftAlt))
            (GameObject as ITransform).Transform.Position += -Input.MouseDelta.XY_ * _speed;
    }
    
}

public interface ICameraController : ITransform {
    CameraController CameraController { get; set; }
}
