using ExampleGame.Entities;
using GameEngine.Core.SourceGenerator;
using GameEngine.Core.Components;
using GameEngine.Core.Core;
using GameEngine.Core.Debugging;
using GameEngine.Core.SceneManagement;

namespace ExampleGame.Components; 

[RequireComponent(typeof(Transform))]
public partial class Blaster : Component {

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
        Bullet bullet = new Bullet() {
            Transform = { Position = Transform.Position }
        };
        Hierarchy.AddEntity(bullet);
        Console.LogSuccess("Spawned Bullet!");
    }
    
}
