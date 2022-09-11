//using System;
//using System.Collections.Generic;
//using GameEngine.Core.AssetManagement;
//using GameEngine.Core.Guard;
//
//namespace GameEngine.Core.Rendering.Textures;
//
//public static class TextureRegister {
//    
//    private static readonly Dictionary<Guid, Texture> _textureRegister = new();
//    private static Texture2D _missingTexture2D = null!;
//    
//    private static void Register(Guid guid, Texture texture) {
//        Throw.If(_textureRegister.ContainsKey(guid), "duplicate texture");
//        _textureRegister.Add(guid, texture);
//    }
//    
//    public static Texture Get(Guid guid) {
//        if(_textureRegister.TryGetValue(guid, out Texture? texture))
//            return texture;
//        Console.LogWarning($"Texture not found ({guid})");
//        return _missingTexture2D;
//    }
//    
//    public static void Reload() {
//        _textureRegister.Clear();
//        _missingTexture2D = Texture2D.CreateMissingTexture();
//        Console.Log($"Loading textures...");
//        string[] paths = AssetManager.Instance.GetAllFilePathsOfAssetsWithExtension("png");
//        
//        for (int i = 0; i < paths.Length; i++) {
//            string texturePath = paths[i];
//            Guid guid = AssetManager.Instance.GetGuidOfAsset(texturePath);
//            Register(guid, new Texture2D(texturePath));
//            Console.LogSuccess($"Loading textures ({i + 1}/{paths.Length}) '{texturePath}'");
//        }
//    }
//    
//}
