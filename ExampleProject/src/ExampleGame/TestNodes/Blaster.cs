using GameEngine.Core;
using GameEngine.Core.Nodes;
using GameEngine.Core.SceneManagement;
using GameEngine.Core.Serialization;

namespace ExampleGame.Nodes; 

[Arr<Bullet>]
public partial class Blaster : Transform3D {
    
    public bool IsShooting;
    [Serialized] public float Cooldown { get; set; } = 0.1f;
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
        Bullet bullet = New<Bullet>();
        bullet.LocalPosition = this.LocalPosition;
        // Hierarchy.AddEntity(bullet);
        Hierarchy.RegisterNode(bullet, Bullets);
        Console.LogSuccess("Spawned Bullet!");
    }
    
}
