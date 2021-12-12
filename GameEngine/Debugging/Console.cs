using System;

namespace GameEngine.Debug; 

public static class Console {
    
    public static void Log(string message) {
        Log("", message, ConsoleColor.White);
    }
    
    public static void LogWarning(string message) {
        Log("A Waring occured: ", message, ConsoleColor.Yellow);
    }
    
    public static void LogError(string message) {
        Log("An Error occurred: ", message, ConsoleColor.Red);
    }
    
    public static void LogSuccess(string message) {
        Log("Success: ", message, ConsoleColor.Green);
    }
    
    private static void Log(string prefix, string message, ConsoleColor color) {
        ConsoleColor prevColor = System.Console.ForegroundColor;
        System.Console.ForegroundColor = color;
        System.Console.WriteLine($"{prefix}{message}");
        System.Console.ForegroundColor = prevColor;
    }
    
}
