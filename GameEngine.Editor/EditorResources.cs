using GameEngine.Core.Guard;
using GameEngine.Core.Rendering.Textures;

namespace GameEngine.Editor; 

public static class EditorResources {
    
    public const string ENGINE_DIRECTORY = @"D:\Dev\C#\CSharpGameEngine";
    public const string ENGINE_EDITOR_DIRECTORY = ENGINE_DIRECTORY + @"\GameEngine.Editor";
    public const string ENGINE_EDITOR_ASSET_DIRECTORY = ENGINE_EDITOR_DIRECTORY + @"\Assets";
    
    private static readonly Dictionary<string, Texture2D> _iconRegister = new();
    
    public static void Load() {
        Console.Log($"Loading Editor Resources...");
        string[] paths = GetAllFilePathsOfAssetsWithExtension("png");
        for (int i = 0; i < paths.Length; i++) {
            RegisterIcon(Path.GetFileNameWithoutExtension(paths[i]).ToLower(), new Texture2D(paths[i]));
            Console.LogSuccess($"Loading icons ({i + 1}/{paths.Length}) '{paths[i]}'");
        }
    }
    
    public static Texture2D GetIcon(string name) {
        name = name.ToLower();
        if(_iconRegister.TryGetValue(name, out Texture2D texture))
            return texture;
        Console.LogWarning($"Icon not found '{name}'");
        //todo: return missing texture texture
        throw new Exception(name);
    }
    
    private static void RegisterIcon(string name, Texture2D texture) {
        name = name.ToLower();
        Throw.If(_iconRegister.ContainsKey(name), "duplicate texture");
        _iconRegister.Add(name, texture);
    }
    
    private static string[] GetAllFilePathsOfAssetsWithExtension(string fileExtension) {
        // Directory.CreateDirectory(ENGINE_EDITOR_ASSET_DIRECTORY);
        return Directory.GetFiles(ENGINE_EDITOR_ASSET_DIRECTORY, $"*.{fileExtension}", SearchOption.AllDirectories);
    }
    
}
