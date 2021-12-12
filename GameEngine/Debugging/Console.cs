using System;
using GameEngine.Core;

namespace GameEngine.Debugging; 

public static class Console {
    
    public static void Log(string message) {
        if(!Configuration.DoDebugLogs) 
            return;
        Log("", message, ConsoleColor.White);
    }
    
    public static void LogWarning(string message) {
        if(!Configuration.DoDebugWarnings) 
            return;
        Log("A Waring occured: ", message, ConsoleColor.Yellow);
    }
    
    public static void LogError(string message) {
        if(!Configuration.DoDebugErrors) 
            return;
        Log("An Error occurred: ", message, ConsoleColor.Red);
    }
    
    public static void LogSuccess(string message) {
        if(!Configuration.DoDebugSuccess) 
            return;
        Log("Success: ", message, ConsoleColor.Green);
    }
    
    private static void Log(string prefix, string message, ConsoleColor color) {
        ConsoleColor prevColor = System.Console.ForegroundColor;
        System.Console.ForegroundColor = color;
        System.Console.WriteLine($"{prefix}{message}");
        System.Console.ForegroundColor = prevColor;
    }
    
}
