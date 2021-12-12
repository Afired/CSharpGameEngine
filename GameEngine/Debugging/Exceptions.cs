using System;

namespace GameEngine.Debugging; 

public class WindowFailedToLoadException : Exception { }

public class ShaderNotFoundException : Exception {
    public ShaderNotFoundException(string name) : base(name) { }
}

public class ShaderFailedToCompileException : Exception {
    public ShaderFailedToCompileException(string error) : base(error) { }
}
