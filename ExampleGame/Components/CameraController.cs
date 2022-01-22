using GameEngine.AutoGenerator;
using GameEngine.Components;
using GameEngine.Input;

namespace ExampleGame.Components; 

[RequireComponent(typeof(Transform))]
public partial class CameraController : Component {
    
    private float _speed = 0.005f;


    protected override void OnUpdate() {
        Update(0.0001f);
    }

    private void Update(float deltaTime) {
        UpdatePosition();
    }
    
    private void UpdatePosition() {
        if(Input.IsKeyDown(KeyCode.LeftAlt))
            Transform.Position += -Input.MouseDelta.XY_ * _speed;
    }
    
}
