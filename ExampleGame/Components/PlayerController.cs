﻿using GameEngine.AutoGenerator;
using GameEngine.Components;
using GameEngine.Input;
using GameEngine.Numerics;

namespace ExampleGame.Components; 

[RequireComponent(typeof(Transform))]
public partial class PlayerController : Component {
    
    private Vector2 _inputAxis;
    private float _speed = 10f;
    
    
    protected override void OnUpdate() {
        Update(0.0001f);
    }
    
    private void Update(float deltaTime) {
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
        Transform.Position += _inputAxis.XY_.Normalized * deltaTime * _speed;
    }
    
}
