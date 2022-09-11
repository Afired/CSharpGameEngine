using System;
using GameEngine.Core.Rendering.Geometry;
using GameEngine.Core.Rendering.Shaders;
using GameEngine.Core.Rendering.Textures;
using GameEngine.Core.Serialization;

namespace GameEngine.Core.AssetManagement; 

public readonly struct Asset<T> where T : class {
    
    [Serialized(Editor.Hidden)] public Guid Guid { get; }
    
    public T? Get() {
        return AssetDatabase.Get<T>(Guid);
//        if(typeof(Texture).IsAssignableTo(typeof(T))) {
//            Texture tex = TextureRegister.Get(Guid);
//            #if DEBUG
//            if(tex is T t)
//                return t;
//            throw new Exception();
//            #else
//            return tex as T;
//            #endif
//        }
//        if(typeof(Shader).IsAssignableTo(typeof(T))) {
//            Shader tex = ShaderRegister.Get(Guid);
//            #if DEBUG
//            if(tex is T t)
//                return t;
//            throw new Exception();
//            #else
//            return tex as T;
//            #endif
//        }
//        if(typeof(Geometry).IsAssignableTo(typeof(T))) {
//            Geometry? tex = MeshRegister.Get(Guid);
//            #if DEBUG
//            if(tex is T t)
//                return t;
//            throw new Exception();
//            #else
//            return tex as T;
//            #endif
//        }
//        return null;
    }
    
    public Asset(Guid guid) {
        Guid = guid;
    }
    
}
