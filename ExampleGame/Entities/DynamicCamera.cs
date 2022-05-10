using ExampleGame.Components;
using GameEngine.Core.Components;
using GameEngine.Core.Debugging;
using GameEngine.Core.Entities;
using GameEngine.Core.Input;
using GameEngine.Core.Rendering;
using GameEngine.Core.Rendering.Cameras;

namespace ExampleGame.Entities; 

public partial class DynamicCamera : Node, ITransform, ICamera3D, ICameraController, ICamera2D {
    
    protected override void OnAwake() {
        base.OnAwake();
        Camera3D.FieldOfView = 75;
        Camera3D.NearPlaneDistance = 0.01f;
        Camera3D.FarPlaneDistance = 100f;
        RenderingEngine.SetActiveCamera(Camera2D);
    }
    
}
