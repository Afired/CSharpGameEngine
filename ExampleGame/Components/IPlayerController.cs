using GameEngine.Components;
using GameEngine.Core;
using GameEngine.Entities;
using GameEngine.Input;
using GameEngine.Layers;
using GameEngine.Numerics;
using GameEngine.Rendering;
using ImGuiNET;

namespace ExampleGame.Components; 

public class PlayerController : Component {
    
    private Vector2 _inputAxis;
    private float _speed = 10f;
    
    
    public PlayerController(Entity entity) : base(entity) {
        Application.OnUpdate += OnUpdate;
        DefaultOverlayLayer.OnDraw += OnImGui;
    }

    private void OnImGui() {
        ImGui.Begin("PlayerController Component");
        ImGui.SliderFloat("Speed", ref _speed, 0f, 50f);
        ImGui.End();
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
        (Entity as ITransform).Transform.Position += _inputAxis.XY_.Normalized * deltaTime * _speed;
    }
    
}

public interface IPlayerController : ITransform {
    public PlayerController PlayerController { get; set; }
}
