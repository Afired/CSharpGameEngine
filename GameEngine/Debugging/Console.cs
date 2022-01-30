using System;
using GameEngine.Core;

namespace GameEngine.Debugging;

public delegate void OnLog(string message, LogSeverity logSeverity);

public static class Console {

    public static event OnLog OnLog;
    
    public static void Log(string message) {
        if(!Configuration.DoDebugLogs) 
            return;
        Log("", message, ConsoleColor.White, LogSeverity.Normal);
    }
    
    public static void LogWarning(string message) {
        if(!Configuration.DoDebugWarnings) 
            return;
        Log("A Waring occured: ", message, ConsoleColor.Yellow, LogSeverity.Warning);
    }
    
    public static void LogError(string message) {
        if(!Configuration.DoDebugErrors) 
            return;
        Log("An Error occurred: ", message, ConsoleColor.Red, LogSeverity.Error);
    }
    
    public static void LogSuccess(string message) {
        if(!Configuration.DoDebugSuccess) 
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
