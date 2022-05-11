using GameEngine.Core.Debugging;
using GameEngine.Core.Nodes;

namespace ExampleGame.Nodes; 

public partial class Enemy : Transform, IRenderer, ITrigger {
    
    protected override void OnAwake() {
        base.OnAwake();
        Trigger.OnBeginTrigger += OnBeginTrigger;
    }
    
    private void OnBeginTrigger(Trigger other) {
        Console.Log("Enemy hit something!");
    }
    
}
