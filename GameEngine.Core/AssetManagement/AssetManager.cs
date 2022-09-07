using System;
using System.IO;

namespace GameEngine.Core.AssetManagement;

public abstract class AssetManager {
    
    public static AssetManager Instance { get; protected set; }
    
    public abstract string[] GetAllFilePathsOfAssetsWithExtension(string fileExtension);
    
    public Guid GetGuidOfAsset(string assetPath) {
        string metaPath = assetPath + ".meta";
        
        if(File.Exists(metaPath)) {
            try {
                return Guid.Parse(File.ReadAllText(metaPath));
            } catch(Exception exception) {
                Console.LogWarning($"couldn't retrieve GUID of {assetPath} ({exception})");
                return CreateNewMetaWithGuid(assetPath);
            }
        } else {
            return CreateNewMetaWithGuid(assetPath);
        }
    }
    
    private Guid CreateNewMetaWithGuid(string assetPath) {
        string metaPath = assetPath + ".meta";
        Guid guid = Guid.NewGuid();
        File.WriteAllText(metaPath, guid.ToString());
        Console.Log($"created new meta file at {assetPath} | asset:{assetPath} guid:{guid}");
        return guid;
    }
    
}
