using GameEngine.Core.Core;
using GameEngine.Core.Numerics;

namespace GameEngine.Core.Nodes; 

//TODO: proper support for abstract classes: require component attribute without the generation of the component interface but partial extension class
public abstract class BaseCamera : Node {
    
    public Color BackgroundColor { get; set; }
    
    
    public BaseCamera(Node node) : base(node) {
        BackgroundColor = Configuration.DefaultBackgroundColor;
    }
    
    public abstract Matrix4x4 GetProjectionMatrix();
    
}
