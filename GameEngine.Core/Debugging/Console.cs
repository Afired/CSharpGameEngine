using System;

namespace GameEngine.Core.Debugging;

public delegate void OnLog(string message, LogSeverity logSeverity);

public static class Console {

    public static event OnLog? OnLog;
    
    public static void Log(string message) {
        if(!Application.Instance!.Config.DoDebugLogs) 
            return;
        Log("", message, ConsoleColor.White, LogSeverity.Normal);
    }
    
    public static void LogWarning(string message) {
        if(!Application.Instance!.Config.DoDebugWarnings) 
            return;
        Log("Warning: ", message, ConsoleColor.Yellow, LogSeverity.Warning);
    }
    
    public static void LogError(string message) {
        if(!Application.Instance!.Config.DoDebugErrors) 
            return;
        Log("Error: ", message, ConsoleColor.Red, LogSeverity.Error);
    }
    
    public static void LogSuccess(string message) {
        if(!Application.Instance!.Config.DoDebugSuccess) 
            return;
        Log("Success: ", message, ConsoleColor.Green, LogSeverity.Success);
    }
    
    private static void Log(string prefix, string message, ConsoleColor color, LogSeverity logSeverity) {
        ConsoleColor prevColor = System.Console.ForegroundColor;
        System.Console.ForegroundColor = color;
        System.Console.WriteLine($"{prefix}{message}");
        System.Console.ForegroundColor = prevColor;
        OnLog?.Invoke(message, logSeverity);
    }
    
}
