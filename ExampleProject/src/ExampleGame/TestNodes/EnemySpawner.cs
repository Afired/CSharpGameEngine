using GameEngine.Core;
using GameEngine.Core.Nodes;
using GameEngine.Core.SceneManagement;
using GameEngine.Core.Serialization;

namespace ExampleGame.Nodes;

[Arr<PhysicsQuad>]
public partial class PhysicsSpawner : Transform3D {
    
    [Serialized] private float SpawnInterval { get; set; } = 0.1f;
    [Serialized] private float TimeUntilNextSpawn { get; set; } = 0f;
    
    protected override void OnUpdate() {
        if(TimeUntilNextSpawn > 0) {
            TimeUntilNextSpawn -= Time.DeltaTime;
            return;
        }
        
        SpawnEnemy();
        TimeUntilNextSpawn = SpawnInterval;
    }
    
    private void SpawnEnemy() {
        PhysicsQuad physicsQuad = New<PhysicsQuad>();
        physicsQuad.LocalPosition = LocalPosition;
        Hierarchy.RegisterNode(physicsQuad, PhysicsQuads);
    }
    
}
