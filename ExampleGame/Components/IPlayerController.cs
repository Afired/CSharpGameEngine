using GameEngine.Components;
using GameEngine.Core;
using GameEngine.Input;
using GameEngine.Numerics;

namespace ExampleGame.Components; 

public class PlayerController : Component {
    
    private Vector2 _inputAxis;
    private float _speed = 10f;
    
    
    public PlayerController(GameObject gameObject) : base(gameObject) {
        Game.OnUpdate += OnUpdate;
    }
    
    private void OnUpdate(float deltaTime) {
        UpdateInputAxis();
        UpdatePosition(deltaTime);
    }

    private void UpdateInputAxis() {
        _inputAxis = new Vector2();
        _inputAxis.X += Input.IsKeyDown(KeyCode.A) ? -1 : 0;
        _inputAxis.X += Input.IsKeyDown(KeyCode.D) ? 1 : 0;
        _inputAxis.Y += Input.IsKeyDown(KeyCode.S) ? -1 : 0;
        _inputAxis.Y += Input.IsKeyDown(KeyCode.W) ? 1 : 0;
    }

    private void UpdatePosition(float deltaTime) {
        (GameObject as ITransform).Transform.Position += _inputAxis.X_Y * deltaTime * _speed;
    }

}

public interface IPlayerController : ITransform {
    public PlayerController PlayerController { get; set; }
}
