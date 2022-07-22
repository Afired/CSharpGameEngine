using System.IO;
using System.Linq;
using GameEngine.Core.Rendering.Geometry;
using GameEngine.Core.Rendering.Shaders;
using GameEngine.Core.Rendering.Textures;

namespace GameEngine.Core.AssetManagement; 

public static class AssetManager {
    
    public static string[] GetAllTexturePaths() =>
        GetAllFilePathsOfAssetsWithExtension("png");
    
    public static string[] GetAllShaderPaths() =>
        GetAllFilePathsOfAssetsWithExtension("glsl");
    
    public static string[] GetAllModelPaths() =>
        GetAllFilePathsOfAssetsWithExtension("obj").Concat(GetAllFilePathsOfAssetsWithExtension("gltf")).ToArray();
    
    public const string ENGINE_DIRECTORY = @"D:\Dev\C#\CSharpGameEngine";
    public const string ENGINE_CORE_DIRECTORY = ENGINE_DIRECTORY + @"\GameEngine.Core";
    public const string ENGINE_CORE_ASSET_DIRECTORY = ENGINE_CORE_DIRECTORY + @"\Assets";
    
    // public const string PROJECT_DIRECTORY = @"D:\Dev\C#\CSharpGameEngine\ExampleProject";
    // public const string PROJECT_ASSET_DIRECTORY = PROJECT_DIRECTORY + @"\Assets";
    
    public static string? ProjectDirectory { get; private set; }
    public static string? ProjectAssetDirectory => ProjectDirectory is null ? null : ProjectDirectory + @"\Assets";
    
    public static string[] GetAllFilePathsOfAssetsWithExtension(string fileExtension) {
        // Directory.CreateDirectory(ENGINE_CORE_ASSET_DIRECTORY);
        // Directory.CreateDirectory(PROJECT_ASSET_DIRECTORY);
        
        string[] engineCorePaths = Directory.GetFiles(ENGINE_CORE_ASSET_DIRECTORY, $"*.{fileExtension}", SearchOption.AllDirectories);

        if(ProjectAssetDirectory is null)
            return engineCorePaths;
        else
            return engineCorePaths.Concat(
                Directory.GetFiles(ProjectAssetDirectory, $"*.{fileExtension}", SearchOption.AllDirectories)
            ).ToArray();
    }
    
    public static void OpenProject(string projectPath) {
        //TODO: validate etc.
        ProjectDirectory = projectPath;
        TextureRegister.Reload();
        ShaderRegister.Reload();
        MeshRegister.Reload();
        //TODO: Reload assembly
    }
    
}
