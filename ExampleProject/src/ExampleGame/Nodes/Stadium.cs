using GameEngine.Core.Nodes;
using GameEngine.Core.Numerics;

namespace ExampleGame.Nodes; 

public partial class Stadium : Scene, Has<Camera2D>, Arr<Player>, Has<Ball> {
    
    protected override void OnAwake() {
        base.OnAwake();
        Start();
    }
    
    private void Start() {
        Ball.LocalPosition = Vector3.Zero;
        Ball.Velocity = new Vector2(5, 5);
    }
    
}
