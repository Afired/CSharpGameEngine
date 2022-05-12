using GameEngine.Core.Core;
using GameEngine.Core.Debugging;
using GameEngine.Core.Nodes;
using GameEngine.Core.SceneManagement;
using GameEngine.Core.Serialization;

namespace ExampleGame.Nodes; 

public partial class Blaster : Transform {
    
    public bool IsShooting;
    [Serialized] public float Cooldown { get; set; }= 0.1f;
    [Serialized] public float CurrentCooldown { get; set; } = 0;
    
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
            LocalPosition = this.Position
        };
        Hierarchy.AddEntity(bullet);
        Console.LogSuccess("Spawned Bullet!");
    }
    
}
