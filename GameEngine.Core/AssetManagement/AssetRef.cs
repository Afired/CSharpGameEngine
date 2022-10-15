using System;
using GameEngine.Core.Serialization;

namespace GameEngine.Core.AssetManagement; 

public readonly struct AssetRef<T> where T : class, IAsset {
    
    [Serialized(Editor.Hidden)] public Guid Guid { get; }
    
    public T? Get() {
        return AssetDatabase.Get<T>(Guid);
    }
    
    public AssetRef(Guid guid) {
        Guid = guid;
    }
    
    public static implicit operator T?(AssetRef<T> assetRef) {
        return assetRef.Get();
    }
    
    //public static implicit operator AssetRef<T>(T asset) {
    //    return new AssetRef<T>(asset.Guid);
    //}
    
}
