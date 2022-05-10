using GameEngine.Core.Components;
using GameEngine.Core.Entities;
using GameEngine.Core.Rendering;
using GameEngine.Core.Rendering.Cameras;

namespace ExampleGame.Entities; 

public partial class StaticCamera : Node, ITransform, ICamera2D {
    
    protected override void OnAwake() {
        base.OnAwake();
        RenderingEngine.SetActiveCamera(Camera2D);
    }
    
}
