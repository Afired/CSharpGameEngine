using GameEngine.Core.Rendering;
using GameEngine.Core.Serialization;
using GlmNet;

namespace GameEngine.Core.Nodes; 

//TODO: proper support for abstract classes: require component attribute without the generation of the component interface but partial extension class
public abstract partial class BaseCamera : Transform3D {
    
    [Serialized] public bool IsMainCamera = true;
    [Serialized] public Color BackgroundColor { get; set; } = Configuration.DefaultBackgroundColor;
    
    public abstract mat4 GLM_GetProjectionMatrix();
    
    protected override void OnAwake() {
        base.OnAwake();
        SetActiveCamera(this);
    }
    
    protected override void OnUpdate() {
        base.OnUpdate();
        if(IsMainCamera)
            SetActiveCamera(this);
    }
    
}
