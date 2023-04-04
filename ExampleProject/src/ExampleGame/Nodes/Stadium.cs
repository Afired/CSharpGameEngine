using GameEngine.Core.Nodes;
using GameEngine.Numerics;

namespace ExampleGame.Nodes; 

[Has<Camera2D>]
[Arr<Player>]
[Has<Ball>]
public partial class Stadium : Scene {
    
    protected override void OnAwake() {
        base.OnAwake();
        Start();
    }
    
    private void Start() {
        Ball.LocalPosition = Vec3<float>.Zero;
        Ball.Velocity = new Vec2<float>(5, 5);
    }
    
}
