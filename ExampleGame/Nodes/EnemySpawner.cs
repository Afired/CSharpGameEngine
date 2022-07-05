using GameEngine.Core.Core;
using GameEngine.Core.Nodes;
using GameEngine.Core.Serialization;

namespace ExampleGame.Nodes;

public partial class EnemySpawner : Node, Arr<SpawnPoint> {
    
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
        // Enemy newEnemy = New<Enemy>();
        // Scene.Current?.Add(newEnemy);
        // newEnemy.Position = SpawnPoints.GetRandom()?.Position ?? Vector3.Zero;
    }
    
}