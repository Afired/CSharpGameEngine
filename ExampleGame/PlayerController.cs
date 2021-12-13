using System;
using GameEngine;
using GameEngine.Core;
using GameEngine.Input;
using GameEngine.Numerics;
using Console = GameEngine.Debugging.Console;

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
        UpdateRotation(deltaTime);
    }

    private void UpdateInputAxis() {
        _inputAxis = new Vector2();
        _inputAxis.X += Input.IsKeyDown(KeyCode.A) ? -1 : 0;
        _inputAxis.X += Input.IsKeyDown(KeyCode.D) ? 1 : 0;
        _inputAxis.Y += Input.IsKeyDown(KeyCode.S) ? -1 : 0;
        _inputAxis.Y += Input.IsKeyDown(KeyCode.W) ? 1 : 0;
    }

    private void UpdatePosition(float deltaTime) {
        _objectToBeMoved.Transform.Position += new Vector3(0, 0, _inputAxis.Y) * deltaTime * _speed;
    }

    private void UpdateRotation(float deltaTime) {
        _objectToBeMoved.Transform.Rotation += new Vector3(0, deltaTime * 4, 0);
        Console.Log(_objectToBeMoved.Transform.Rotation.ToString());
    }
    
}
