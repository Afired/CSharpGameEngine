using System;
using System.Diagnostics;
using System.Threading;

namespace GameEngine.Core;

public delegate void OnUpdate(float deltaTime);
public delegate void OnFixedUpdate(float fixedDeltaTime);

public class Game {
    
    public static event OnUpdate OnUpdate;
    public static event OnFixedUpdate OnFixedUpdate;
    public bool IsRunning;
    
    
    public void Initialize() {
        IsRunning = true;
        new Thread(new ThreadStart(UpdateLoop)).Start();
        new Thread(new ThreadStart(FixedUpdateLoop)).Start();
    }
    
    private void UpdateLoop() {
        Stopwatch stopwatch = new();
        while(IsRunning) {
            float elapsedTime = (float) stopwatch.Elapsed.TotalSeconds;
            if(Configuration.TargetFrameRate > 0) {
                TimeSpan timeOut = TimeSpan.FromSeconds(1 / Configuration.TargetFrameRate - elapsedTime);
                if(timeOut.TotalSeconds > 0) {
                    Thread.Sleep(timeOut);
                    elapsedTime = (float) stopwatch.Elapsed.TotalSeconds;
                }
            }
            stopwatch.Restart();
            OnUpdate?.Invoke(elapsedTime);
        }
    }
    
    private void FixedUpdateLoop() {
        Stopwatch stopwatch = new();
        while(IsRunning) {
            float elapsedTime = (float) stopwatch.Elapsed.TotalSeconds;
            TimeSpan timeOut = TimeSpan.FromSeconds(Configuration.FixedTimeStep - elapsedTime);
            Thread.Sleep(timeOut);
            stopwatch.Restart();
            OnFixedUpdate?.Invoke(Configuration.FixedTimeStep);
        }
    }
    
}
