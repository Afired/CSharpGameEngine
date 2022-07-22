using System.IO;
using System.Linq;

namespace GameEngine.Core.AssetManagement; 

public static class AssetManager {
    
    public static string[] GetAllTexturePaths() =>
        GetAllFilePathsOfAssetsWithExtension("png");
    
    public static string[] GetAllShaderPaths() =>
        GetAllFilePathsOfAssetsWithExtension("glsl");
    
    public const string ENGINE_DIRECTORY = @"D:\Dev\C#\CSharpGameEngine";
    public const string ENGINE_CORE_DIRECTORY = ENGINE_DIRECTORY + @"\GameEngine.Core";
    public const string ENGINE_CORE_ASSET_DIRECTORY = ENGINE_CORE_DIRECTORY + @"\Assets";
    
    public const string PROJECT_DIRECTORY = @"D:\Dev\C#\CSharpGameEngine\ExampleProject";
    public const string PROJECT_ASSET_DIRECTORY = PROJECT_DIRECTORY + @"\Assets";
    
    public static string[] GetAllFilePathsOfAssetsWithExtension(string fileExtension) {
        // Directory.CreateDirectory(ENGINE_CORE_ASSET_DIRECTORY);
        // Directory.CreateDirectory(PROJECT_ASSET_DIRECTORY);
        return 
            Directory.GetFiles(ENGINE_CORE_ASSET_DIRECTORY, $"*.{fileExtension}", SearchOption.AllDirectories).Concat(
                Directory.GetFiles(PROJECT_ASSET_DIRECTORY, $"*.{fileExtension}", SearchOption.AllDirectories)
            ).ToArray();
    }
    
}
