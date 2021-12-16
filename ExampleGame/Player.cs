using ExampleGame;
using GameEngine.Geometry;

namespace GameEngine.Components; 

public class Player : GameObject, ITransform, IPlayerController, IPyramid {
    public Transform Transform { get; }
    public PlayerController PlayerController { get; set; }
    public Pyramid Pyramid { get; set; }

    public Player() {
        Transform = new Transform(this);
        PlayerController = new PlayerController(this);
        Pyramid = new Pyramid(this);
    }
    
}
