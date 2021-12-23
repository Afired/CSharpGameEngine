using ExampleGame.Components;
using GameEngine.Components;
using GameEngine.Entities;
using GameEngine.Rendering.Cameras;

namespace ExampleGame.Entities; 

public class Player : Entity, ITransform, IPlayerController, ICamera3D, ICameraController, ICamera2D {
    
    public Transform Transform { get; set; }
    public PlayerController PlayerController { get; set; }
    public Camera3D Camera3D { get; set; }
    public CameraController CameraController { get; set; }
    public Camera2D Camera2D { get; set; }
    
    
    public Player() {
        Transform = new Transform(this);
        PlayerController = new PlayerController(this);
        Camera3D = new Camera3D(this, 75, 0.01f, 100f);
        CameraController = new CameraController(this);
        Camera2D = new Camera2D(this, 50);
    }
    
}
