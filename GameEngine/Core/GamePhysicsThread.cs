using System;
using System.Diagnostics;
using System.Threading;

namespace GameEngine.Core;

public delegate void OnFixedUpdate(float fixedDeltaTime);

public sealed partial class Game {
    
    public static event OnFixedUpdate OnFixedUpdate;
    
    private void FixedUpdateLoop() {
        Stopwatch stopwatch = new();
        while(_isRunning) {
            float elapsedTime = (float) stopwatch.Elapsed.TotalSeconds;
            TimeSpan timeOut = TimeSpan.FromSeconds(Configuration.FixedTimeStep - elapsedTime);
            Thread.Sleep(timeOut);
            stopwatch.Restart();
            OnFixedUpdate?.Invoke(Configuration.FixedTimeStep);
        }
    }
    
}
