using GameEngine.Components;
using GameEngine.Core;
using GameEngine.Entities;
using GameEngine.Input;

namespace ExampleGame.Components; 

public class CameraController : Component {
    
    private float _speed = 0.005f;
    
    
    public CameraController(Entity entity) : base(entity) {
        Application.OnUpdate += OnUpdate;
    }
    
    private void OnUpdate(float deltaTime) {
        UpdatePosition();
    }
    
    private void UpdatePosition() {
        if(Input.IsKeyDown(KeyCode.LeftAlt))
            (Entity as ITransform).Transform.Position += -Input.MouseDelta.XY_ * _speed;
    }
    
}

public interface ICameraController : ITransform {
    CameraController CameraController { get; set; }
}
