using GameEngine.Components;
using GameEngine.Entities;
using GameEngine.Rendering;
using GameEngine.Rendering.Cameras;

namespace ExampleGame.Entities; 

public partial class StaticCamera : Entity, ITransform, ICamera2D {
    
    protected override void OnAwake() {
        base.OnAwake();
        RenderingEngine.SetActiveCamera(Camera2D);
    }
    
}
