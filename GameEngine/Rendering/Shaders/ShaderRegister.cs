using System.Collections.Generic;
using GameEngine.Debugging;

namespace GameEngine.Rendering.Shaders; 

public static class ShaderRegister {
    
    private static Dictionary<string, Shader> _shaderRegister;
    
    
    static ShaderRegister() {
        _shaderRegister = new Dictionary<string, Shader>();
    }

    public static void Register(string name, Shader shader) {
        //todo: throw new duplicate shader exception
        _shaderRegister.Add(name, shader);
    }

    public static Shader Get(string name) {
        if(_shaderRegister.TryGetValue(name, out Shader shader))
            return shader;
        else
            throw new ShaderNotFoundException(name);
    }
    
}
