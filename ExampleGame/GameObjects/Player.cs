using ExampleGame;
using GameEngine.Rendering.Cameras;

namespace GameEngine.Components; 

public class Player : GameObject, ITransform, IPlayerController, ICamera3D, ICameraController {
    
    public Transform Transform { get; set; }
    public PlayerController PlayerController { get; set; }
    public Camera3D Camera3D { get; set; }
    public CameraController CameraController { get; set; }
    
    
    public Player() {
        Transform = new Transform(this);
        PlayerController = new PlayerController(this);
        Camera3D = new Camera3D(this, 75, 0.01f, 100f);
        CameraController = new CameraController(this);
    }
    
}
