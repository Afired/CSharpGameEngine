using GameEngine.Components;
using GameEngine.Core;
using GameEngine.Numerics;

namespace GameEngine.Rendering.Cameras; 

public abstract class BaseCamera : Component {
    
    public Color BackgroundColor { get; set; }
    
    
    public BaseCamera(GameObject gameObject) : base(gameObject) {
        BackgroundColor = Configuration.DefaultBackgroundColor;
    }
    
    public abstract Matrix4x4 GetProjectionMatrix();
}
