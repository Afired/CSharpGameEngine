﻿using GameEngine.AutoGenerator;
using GameEngine.Components;
using GameEngine.Core;
using GameEngine.Input;
using GameEngine.Numerics;

namespace ExampleGame.Components; 

[RequireComponent(typeof(Transform))]
public partial class PlayerController : Component {
    
    private Vector2 _inputAxis;
    private float _speed = 10f;
    
    
    protected override void OnUpdate() {
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
        Transform.Position += _inputAxis.XY_.Normalized * Time.DeltaTime * _speed;
    }
    
}
