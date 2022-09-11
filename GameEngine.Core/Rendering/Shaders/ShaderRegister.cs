//using System;
//using System.Collections.Generic;
//using System.IO;
//using GameEngine.Core.AssetManagement;
//using GameEngine.Core.Debugging;
//using GameEngine.Core.Guard;
//using GameEngine.Core.Rendering.Textures;
//
//namespace GameEngine.Core.Rendering.Shaders; 
//
//public static class ShaderRegister {
//    
//    private static readonly Dictionary<Guid, Shader> _shaderRegister = new();
//    private static Shader _invalidShaderShader;
//    
//    public static void Register(Guid guid, Shader shader) {
//        Throw.If(_shaderRegister.ContainsKey(guid), "duplicate shader");
//        _shaderRegister.Add(guid, shader);
//    }
//
//    public static Shader Get(Guid guid) {
//        if(_shaderRegister.TryGetValue(guid, out Shader shader))
//            return shader;
//        Console.LogWarning($"Shader not found '{guid}'");
//        return _invalidShaderShader;
//    }
//    
//    public static void Reload() {
//        _shaderRegister.Clear();
//        Console.Log($"Compiling shaders...");
//        _invalidShaderShader = InvalidShader.Create();
//        string[] paths = AssetManager.Instance.GetAllFilePathsOfAssetsWithExtension("glsl");
//        for (int i = 0; i < paths.Length; i++) {
//            Register(AssetManager.Instance.GetGuidOfAsset(paths[i]), new Shader(paths[i]));
//            Console.LogSuccess($"Compiling shaders ({i + 1}/{paths.Length}) '{paths[i]}'");
//        }
//    }
//        
//}
