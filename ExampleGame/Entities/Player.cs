using ExampleGame.Components;
using GameEngine.Components;
using GameEngine.Debugging;
using GameEngine.Entities;
using GameEngine.Input;
using GameEngine.Rendering;
using GameEngine.Rendering.Cameras;

namespace ExampleGame.Entities; 

public partial class Player : Entity, ITransform, IPlayerController, ICamera3D, ICameraController, ICamera2D {
    
    protected override void OnAwake() {
        base.OnAwake();
        Camera2D.Zoom = 50;
        Camera3D.FieldOfView = 75;
        Camera3D.NearPlaneDistance = 0.01f;
        Camera3D.FarPlaneDistance = 100f;
        RenderingEngine.SetActiveCamera(Camera2D);
    }

    protected override void OnUpdate() {
        base.OnUpdate();
        if(Input.IsKeyDown(KeyCode.E)) {
            Console.LogError($"Players position is x:{Transform.Position.X} y:{Transform.Position.Y} z:{Transform.Position.Z}");
        }
    }
    
}
