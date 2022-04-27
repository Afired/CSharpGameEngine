using ExampleGame.Entities;
using GameEngine.Components;
using GameEngine.Debugging;

namespace ExampleGame.Components; 

public partial class EnemyTrigger : Trigger {
    
    protected override void OnBeginTrigger(Trigger other) {
        if(other.Entity is Player player) {
            Console.Log("enemy reached player");
        }
    }
    
}
