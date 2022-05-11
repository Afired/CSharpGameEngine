using GameEngine.Core.Core;
using GameEngine.Core.Debugging;
using GameEngine.Core.Nodes;
using GameEngine.Core.SceneManagement;

namespace ExampleGame.Nodes; 

public partial class Blaster : Transform {
    
    public bool IsShooting;
    public float Cooldown = 0.1f;
    public float CurrentCooldown = 0;
    
    protected override void OnUpdate() {
        if(CurrentCooldown > 0) {
            CurrentCooldown -= Time.DeltaTime;
            return;
        }
        
        if(!IsShooting)
            return;
        
        Shoot();
        CurrentCooldown = Cooldown;
    }
    
    private void Shoot() {
        Bullet bullet = new Bullet {
            Position = this.Position
        };
        Hierarchy.AddEntity(bullet);
        Console.LogSuccess("Spawned Bullet!");
    }
    
}
