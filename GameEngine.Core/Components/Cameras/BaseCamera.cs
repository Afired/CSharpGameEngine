using GameEngine.Core.SourceGenerator;
using GameEngine.Core.Components;
using GameEngine.Core.Core;
using GameEngine.Core.Entities;
using GameEngine.Core.Numerics;

namespace GameEngine.Core.Rendering.Cameras; 

//TODO: proper support for abstract classes: require component attribute without the generation of the component interface but partial extension class
public abstract class BaseCamera : Component {
    
    public Color BackgroundColor { get; set; }
    
    
    public BaseCamera(Entity entity) : base(entity) {
        BackgroundColor = Configuration.DefaultBackgroundColor;
    }
    
    public abstract Matrix4x4 GetProjectionMatrix();
    
}
