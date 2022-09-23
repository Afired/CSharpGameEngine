using GameEngine.Core.AssetManagement;

namespace GameEngine.Editor; 

public class EditorAssetManager : AssetManager {

    public static void Init() {
        Instance = new EditorAssetManager();
    }

    private EditorAssetManager() { }
    
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
    
    // currently only looks for assets in project asset directory
    public override string? GetMetaPath(Guid guid) {
        
        if(Project.Current is null)
            return null;
        
        foreach(string filePath in Directory.EnumerateFiles(Project.Current.ProjectAssetDirectory, "*.meta", SearchOption.AllDirectories)) {

            foreach(string line in File.ReadLines(filePath)) {
                if(Guid.TryParse(line, out Guid currentGuid) && guid == currentGuid)
                    return filePath;
                break;
            }
                
        }
        return null;
    }
    
    public override string? GetAssetPath(Guid guid) {
        string? metaPath = GetMetaPath(guid);
        if(metaPath is null)
            return null;
        
        string assetPath = Path.Combine(Path.GetDirectoryName(metaPath), Path.GetFileNameWithoutExtension(metaPath));
        
        if(File.Exists(assetPath))
            return assetPath;
        
        return null;
    }
    
}
