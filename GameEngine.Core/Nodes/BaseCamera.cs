using GameEngine.Core.Core;
using GameEngine.Core.Numerics;
using GameEngine.Core.Rendering;
using GameEngine.Core.Serialization;

namespace GameEngine.Core.Nodes; 

//TODO: proper support for abstract classes: require component attribute without the generation of the component interface but partial extension class
public abstract partial class BaseCamera : Transform {
    
    [Serialized] public Color BackgroundColor { get; set; } = Configuration.DefaultBackgroundColor;
    public abstract Matrix4x4 GetProjectionMatrix();

    protected override void OnAwake() {
        base.OnAwake();
        RenderingEngine.SetActiveCamera(this);
    }
    
}
