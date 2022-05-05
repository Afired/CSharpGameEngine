using GameEngine.Core.AutoGenerator;
using GameEngine.Core.Components;
using GameEngine.Core.Input;
using GameEngine.Core.Numerics;

namespace ExampleGame.Components; 

[RequireComponent(typeof(Transform))]
public partial class CameraController : Component {
    
    private float _speed = 0.005f;


    protected override void OnUpdate() {
        UpdatePosition();
    }
    
    private void UpdatePosition() {
        if(Input.IsKeyDown(KeyCode.LeftAlt))
            Transform.Position -= new Vector3(Input.MouseDelta.X, Input.MouseDelta.Y, 0) * _speed;
    }
    
}
