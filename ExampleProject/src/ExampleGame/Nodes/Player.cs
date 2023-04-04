using GameEngine.Core;
using GameEngine.Core.Input;
using GameEngine.Core.Nodes;
using GameEngine.Numerics;
using GameEngine.Core.Serialization;

namespace ExampleGame.Nodes; 

[Has<Paddle>]
[Has<Trigger>]
public partial class Player : Node {
    
    [Serialized] public int Score { get; private set; }
    [Serialized] public string? Name { get; init; }
    [Serialized] public float Speed { get; init; } = 10f;
    [Serialized] public KeyCode MoveRightKey { get; init; }
    [Serialized] public KeyCode MoveLeftKey { get; init; }
    
    protected override void OnAwake() {
        base.OnAwake();
        Trigger.OnBeginTrigger += OnEnterScoreTrigger;
    }
    
    private void OnEnterScoreTrigger(Trigger other) {
        Console.Log(other.ToString());
    }
    
    protected override void OnUpdate() {
        base.OnUpdate();
        
        float x = 0;
        x += Input.IsKeyDown(MoveRightKey) ? 1 : 0;
        x += Input.IsKeyDown(MoveLeftKey) ? -1 : 0;
        x *= Time.DeltaTime * Speed;
        Paddle.LocalPosition += new Vec3<float>(x, 0, 0);
    }
    
}
