using System;
using GameEngine.Core.Serialization;

namespace GameEngine.Core.AssetManagement; 

public readonly struct Asset<T> where T : class {
    
    [Serialized(Editor.Hidden)] public Guid Guid { get; }
    
    public T? Get() {
        return AssetDatabase.Get<T>(Guid);
    }
    
    public Asset(Guid guid) {
        Guid = guid;
    }
    
}
