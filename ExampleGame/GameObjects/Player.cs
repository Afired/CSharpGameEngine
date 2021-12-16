using ExampleGame;

namespace GameEngine.Components; 

public class Player : GameObject, ITransform, IPlayerController, IGeometry, IRenderer {
    public Transform Transform { get; set; }
    public PlayerController PlayerController { get; set; }
    public Geometry Geometry { get; set; }
    public Renderer Renderer { get; set; }
    
    
    public Player() {
        Transform = new Transform(this);
        PlayerController = new PlayerController(this);
        Geometry = new Geometry(this);
        Renderer = new Renderer(this);
    }
    
}
