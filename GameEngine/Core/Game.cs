using System.Diagnostics;
using System.Threading;

namespace GameEngine.Core;

public delegate void OnUpdate(float deltaTime);

public class Game {
    
    public static event OnUpdate OnUpdate;
    public bool IsRunning;
    
    
    public void Initialize() {
        IsRunning = true;
        new Thread(new ThreadStart(UpdateLoop)).Start();
    }
    
    private void UpdateLoop() {
        Stopwatch stopwatch = new();
        while(IsRunning) {
            float deltaTime = (float) stopwatch.Elapsed.TotalSeconds;
            stopwatch.Restart();
            OnUpdate?.Invoke(deltaTime);
        }
    }
    
}
