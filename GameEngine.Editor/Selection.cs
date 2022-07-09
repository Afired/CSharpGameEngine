namespace GameEngine.Editor; 

public static class Selection {
    
    public static object? Current => _currentRef.Target;
    private static readonly WeakReference _currentRef = new(null);
    
    public static void Select<T>(T? obj) where T : class {
        _currentRef.Target = obj;
    }
    
    public static void Clear() {
        _currentRef.Target = null;
    }
    
}
