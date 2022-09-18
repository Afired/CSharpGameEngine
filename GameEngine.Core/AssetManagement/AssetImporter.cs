using System;
using System.Collections.Generic;

namespace GameEngine.Core.AssetManagement; 

public abstract class AssetImporter<TAsset> : IAssetImporter where TAsset : IAsset {
    
    public abstract string[] GetExtensions();
    public abstract TAsset? Import(string path);
    public abstract IEnumerable<TAsset?> Import(string[] paths);
    
    public bool TryImport(string path, out TAsset? asset) {
        asset = Import(path);
        return asset is not null;
    }
    
    Type IAssetImporter.GetAssetType() {
        return typeof(TAsset);
    }
    
    IAsset? IAssetImporter.ImportInternal(string path) {
        return Import(path);
    }
    
    IEnumerable<IAsset?> IAssetImporter.ImportInternal(string[] paths) {
        foreach(TAsset? asset in Import(paths)) {
            yield return asset;
        }
    }
    
}
