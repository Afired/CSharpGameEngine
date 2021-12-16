using GameEngine.Components;
using GameEngine.Numerics;

namespace GameEngine; 

public class Transform : Component {
    
    public Vector3 Position { get; set; }
    public Vector3 Scale { get; set; }
    public Quaternion Rotation { get; set; }
    
    
    public Transform(GameObject gameObject) : base(gameObject) {
        Position = new Vector3(0f, 0f, 0f);
        Scale = new Vector3(1f, 1f, 1f);
        Rotation = new Quaternion(0f, 0f, 0f, 1f).Normalized;
    }
    
}
