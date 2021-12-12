using System.Numerics;
using GameEngine;
using GameEngine.Core;
using GameEngine.Debugging;
using GameEngine.Input;

namespace ExampleGame; 

public class PlayerController {
    
    private ITransform _objectToBeMoved;
    private Vector2 _inputAxis;
    private float _speed = 10f;
    
    
    public PlayerController(ITransform objectToBeMoved) {
        _objectToBeMoved = objectToBeMoved;
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
        Console.LogSuccess(_inputAxis.X + " | " + _inputAxis.Y);
    }

    private void UpdatePosition(float deltaTime) {
        _objectToBeMoved.Transform.Position.X += _inputAxis.X * deltaTime * _speed;
        _objectToBeMoved.Transform.Position.Y += _inputAxis.Y * deltaTime * _speed;
    }
    
}
