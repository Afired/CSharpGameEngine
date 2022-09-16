using System;
using System.Collections.Generic;

namespace GameEngine.Core.AssetManagement; 

public interface IAsset {
    
    public static abstract IAsset Default(Type assetType);
    
    public static abstract IAsset DefaultGen<T>() where T : IAsset, new();
    
    public static abstract string[] Extensions { get; }
    
    public static abstract void /*IEnumerable<IAsset>*/ LoadAssets(string[] paths);
    
}
