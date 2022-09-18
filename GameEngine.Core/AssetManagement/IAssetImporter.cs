using System;
using System.Collections.Generic;

namespace GameEngine.Core.AssetManagement;

public interface IAssetImporter {
    
    public abstract string[] GetExtensions();
    internal abstract Type GetAssetType();
    internal abstract IAsset? ImportInternal(string path);
    internal abstract IEnumerable<IAsset?> ImportInternal(string[] paths);
    
}
