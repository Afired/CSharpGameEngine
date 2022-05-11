using GameEngine.Core.Core;
using GameEngine.Core.Numerics;
using GameEngine.Core.Rendering;

namespace GameEngine.Core.Nodes; 

//TODO: proper support for abstract classes: require component attribute without the generation of the component interface but partial extension class
public abstract partial class BaseCamera : Transform {

    public Color BackgroundColor { get; set; } = Configuration.DefaultBackgroundColor;
    public abstract Matrix4x4 GetProjectionMatrix();
    
}
