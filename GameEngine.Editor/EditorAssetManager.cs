using GameEngine.Core.AssetManagement;

namespace GameEngine.Editor; 

public class EditorAssetManager : AssetManager {

    public static void Init() {
        Instance = new EditorAssetManager();
    }

    private EditorAssetManager() { }
    
    // public static string[] GetAllTexturePaths() =>
    //     GetAllFilePathsOfAssetsWithExtension("png");
    //
    // public static string[] GetAllShaderPaths() =>
    //     GetAllFilePathsOfAssetsWithExtension("glsl");
    //
    // public static string[] GetAllModelPaths() =>
    //     GetAllFilePathsOfAssetsWithExtension("obj").Concat(GetAllFilePathsOfAssetsWithExtension("gltf")).ToArray();
    
    public const string ENGINE_DIRECTORY = @"D:\Dev\C#\CSharpGameEngine";
    public const string ENGINE_CORE_DIRECTORY = ENGINE_DIRECTORY + @"\GameEngine.Core";
    public const string ENGINE_CORE_ASSET_DIRECTORY = ENGINE_CORE_DIRECTORY + @"\Assets";
    
    public override string[] GetAllFilePathsOfAssetsWithExtension(string fileExtension) {
        // Directory.CreateDirectory(ENGINE_CORE_ASSET_DIRECTORY);
        // Directory.CreateDirectory(PROJECT_ASSET_DIRECTORY);
        
        string[] engineCorePaths = Directory.GetFiles(ENGINE_CORE_ASSET_DIRECTORY, $"*.{fileExtension}", SearchOption.AllDirectories);
        
        if(Project.Current is null)
            return engineCorePaths;
        else
            return engineCorePaths.Concat(
                Directory.GetFiles(Project.Current.ProjectAssetDirectory, $"*.{fileExtension}", SearchOption.AllDirectories)
            ).ToArray();
    }
    
}
