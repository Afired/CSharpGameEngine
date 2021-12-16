using GameEngine.Components;
using GameEngine.Core;
using GameEngine.Numerics;

namespace GameEngine.Rendering.Cameras; 

public abstract class BaseCamera : GameObject, ITransform {

    public Transform Transform { get; set; }
    public Color BackgroundColor { get; set; }
    
    
    public BaseCamera() {
        BackgroundColor = Configuration.DefaultBackgroundColor;
        Transform = new Transform(this);
    }
    
    public abstract Matrix4x4 GetProjectionMatrix();
}
