using GameEngine.Core;
using GameEngine.Core.Input;
using GameEngine.Core.Nodes;
using GameEngine.Core.Numerics;
using GameEngine.Core.Serialization;

namespace ExampleGame.Nodes; 

public partial class Player : Node, Has<Paddle>, Has<Trigger> {
    
    [Serialized] public int Score { get; private set; }
    [Serialized] public string? Name { get; init; }
    [Serialized] public float Speed { get; init; } = 100f;
    
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
        x += Input.IsKeyDown(KeyCode.D) ? 1 : 0;
        x += Input.IsKeyDown(KeyCode.A) ? -1 : 0;
        x *= Time.DeltaTime * Speed;
        Paddle.Position += new Vector3(x, 0, 0);
    }
    
}
