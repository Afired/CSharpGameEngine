using System;
using System.Diagnostics;
using System.Threading;

namespace GameEngine.Core;

public delegate void OnUpdate(float deltaTime);

public sealed partial class Game {
    
    public static event OnUpdate OnUpdate;
    
    private void UpdateLoop() {
        Stopwatch stopwatch = new();
        while(_isRunning) {
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
    
}
