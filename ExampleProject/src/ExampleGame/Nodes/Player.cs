using GameEngine.Core.Nodes;
using GameEngine.Core.Serialization;

namespace ExampleGame.Nodes; 

public partial class Player : Node, Has<Paddle>, Has<Trigger> {
    
    [Serialized] public int Score { get; private set; }
    
    protected override void OnAwake() {
        base.OnAwake();
        Trigger.OnBeginTrigger += OnEnterScoreTrigger;
    }
    
    private void OnEnterScoreTrigger(Trigger other) {
        Console.Log(other.ToString());
    }
    
}
