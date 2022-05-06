using System.Collections.Generic;
using GameEngine.Core.Debugging;
using GameEngine.Core.Guard;

namespace GameEngine.Core.Rendering.Geometry;

public static class GeometryRegister {
    
    internal static Dictionary<string, Geometry> _geometryRegister;
    
    
    static GeometryRegister() {
        _geometryRegister = new Dictionary<string, Geometry>();
    }

    public static void Register(string name, Geometry shader) {
        name = name.ToLower();
        Throw.If(_geometryRegister.ContainsKey(name), "duplicate geometry");
        _geometryRegister.Add(name, shader);
    }

    public static Geometry Get(string name) {
        if(name is null) {
            Console.LogWarning($"Geometry not found 'null'");
            return null;
        }
        name = name.ToLower();
        if(_geometryRegister.TryGetValue(name, out Geometry shader))
            return shader;
        Console.LogWarning($"Geometry not found '{name}'");
        return null;
    }
    
    public static void Load() {
        Console.Log($"Initializing geometry...");
        float[] quadVertexData = {
            -0.5f, 0.5f, 0.0f, 0.0f, 1.0f,   // top left
            0.5f, 0.5f, 0.0f, 1.0f, 1.0f,    // top right
            -0.5f, -0.5f, 0.0f, 0.0f , 0.0f, // bottom left

            0.5f, 0.5f, 0.0f, 1.0f, 1.0f,   // top right
            0.5f, -0.5f, 0.0f, 1.0f, 0.0f,  // bottom right
            -0.5f, -0.5f, 0.0f, 0.0f, 0.0f, // bottom left
        };
        Register("Quad", new Geometry(quadVertexData));
    }
    
}
