namespace GameEngine.Editor; 

public static class Selection {
    
    public static object? Current { get; private set; }
    
    public static void Select<T>(T? obj) where T : class {
        Current = obj;
    }
    
    public static void Clear() {
        Current = null;
    }
    
}
