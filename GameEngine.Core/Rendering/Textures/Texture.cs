using System;
using GameEngine.Core.AssetManagement;

namespace GameEngine.Core.Rendering.Textures; 

public abstract class Texture : IAsset {
    
    public abstract void Bind(uint slot = 0);
    
    public static unsafe IAsset Default(Type assetType) {
        fixed(void* data = new byte[] {
                  204, 0, 255, 255, 0, 0, 0, 255, 204, 0, 255, 255, 0, 0, 0, 255,
                  0, 0, 0, 255, 204, 0, 255, 255, 0, 0, 0, 255, 204, 0, 255, 255,
                  204, 0, 255, 255, 0, 0, 0, 255, 204, 0, 255, 255, 0, 0, 0, 255,
                  0, 0, 0, 255, 204, 0, 255, 255, 0, 0, 0, 255, 204, 0, 255, 255
              }) {
            return new Texture2D(data, 4, 4);
        }
    }
    
    public static void LoadAssets(string[] paths) {
        for (int i = 0; i < paths.Length; i++) {
            string texturePath = paths[i];
            Guid guid = AssetManager.Instance.GetGuidOfAsset(texturePath);
            AssetDatabase.Load(guid, new Texture2D(texturePath));
            Console.LogSuccess($"Loading textures ({i + 1}/{paths.Length}) '{texturePath}'");
        }
    }
    
}
