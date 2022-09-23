using System;
using System.Collections.Generic;

namespace GameEngine.Core.AssetManagement; 

public abstract class AssetImporter {
    
    public abstract string[] GetExtensions();
    public abstract Type GetAssetType { get; }
    internal abstract IAsset? ImportInternal(string path);
//    internal abstract IEnumerable<IAsset?> ImportInternal(string[] paths);
    
}

public abstract class AssetImporter<TAsset> : AssetImporter where TAsset : IAsset {
    
    public sealed override Type GetAssetType => typeof(TAsset);
    public abstract TAsset? Import(string path);
//    public abstract IEnumerable<TAsset?> Import(string[] paths);
    
    public bool TryImport(string path, out TAsset? asset) {
        asset = Import(path);
        return asset is not null;
    }
    
    internal sealed override IAsset? ImportInternal(string path) {
        return Import(path);
    }
    
    public virtual IEnumerable<TAsset?> Import(string[] paths) {
        foreach(string path in paths) {
            yield return Import(path);
        }
    }
    
//    IEnumerable<IAsset?> AssetImporter.ImportInternal(string[] paths) {
//        foreach(TAsset? asset in Import(paths)) {
//            yield return asset;
//        }
//    }
    
}
