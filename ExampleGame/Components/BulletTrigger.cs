using ExampleGame.Entities;
using GameEngine.Core.Components;
using GameEngine.Core.Debugging;

namespace ExampleGame.Components; 

public partial class BulletTrigger : Trigger {
    
    protected override void OnBeginTrigger(Trigger other) {
        if(other.Entity is Enemy enemy) {
            Console.Log("bullet hit enemy");
        }
    }
    
}
