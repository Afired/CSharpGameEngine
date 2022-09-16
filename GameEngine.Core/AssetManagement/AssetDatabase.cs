using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
//using System.Threading;
//using Assimp;
//using Assimp.Configs;
//using GameEngine.Core.Rendering.Geometry;
//using GameEngine.Core.Rendering.Shaders;
//using GameEngine.Core.Rendering.Textures;
//using Mesh = GameEngine.Core.Rendering.Geometry.Mesh;
//using Texture = GameEngine.Core.Rendering.Textures.Texture;

namespace GameEngine.Core.AssetManagement;

public static class AssetDatabase {
    
    private static readonly Dictionary<Guid, IAsset> _assetCache = new();
    private static readonly List<IAsset> _defaultAssets = new();
    
    public static void Load(Guid guid, IAsset asset) {
        if(_assetCache.ContainsKey(guid)) {
            Console.LogWarning($"failed to load asset with guid {guid}, there already is loaded an asset with that guid");
            return;
        }
        _assetCache.Add(guid, asset);
    }
    
    public static void Unload(Guid guid) {
        if(!_assetCache.ContainsKey(guid)) {
            Console.LogWarning($"failed to load unload asset with guid {guid}, there is no asset loaded with that guid");
            return;
        }
        _assetCache.Remove(guid);
    }
    
    public static void UnloadAll() {
        _assetCache.Clear();
    }
    
    public static T Get<T>(Guid guid) where T : class {
        if(_assetCache.TryGetValue(guid, out IAsset? texture))
            return (T) texture;
        
        //TODO: Refactor handling of defaults
        IAsset? result = _defaultAssets.FirstOrDefault(asset => asset.GetType() == typeof(T));
        result ??= _defaultAssets.FirstOrDefault(asset => asset.GetType().IsAssignableTo(typeof(T)));
        
        if(result is null)
            throw new Exception($"No default asset found for type {typeof(T)}");

        return (T) result;
    }
    
    public static void Reload() {
        UnloadAll();
        
        //TODO: include asset definitions in other assemblies
        IEnumerable<Type> assetTypes = Assembly.GetAssembly(typeof(Application))!.GetTypes().Where(type => type.IsAssignableTo(typeof(IAsset)) && type.IsClass);
        
        foreach(Type assetType in assetTypes) {
            IAsset? defaultAsset = (IAsset?) assetType.GetMethod(nameof(IAsset.Default), BindingFlags.Public | BindingFlags.Static)?.Invoke(null, new object?[] { assetType } );
            
            if(defaultAsset is null)
                continue;
            
            _defaultAssets.Add(defaultAsset);
            
            string[] extensions = (string[]) assetType.GetProperty(nameof(IAsset.Extensions), BindingFlags.Public | BindingFlags.Static)!.GetValue(null)!;
            for(int i = 0; i < extensions.Length; i++) {
                string[] paths = AssetManager.Instance.GetAllFilePathsOfAssetsWithExtension(extensions[i]);
                assetType.GetMethod(nameof(IAsset.LoadAssets), BindingFlags.Public | BindingFlags.Static)!.Invoke(null, new object?[] { paths } );
            }
        }
//        
//        Console.Log($"Loading textures...");
//        Texture2D.MissingTexture = Texture2D.CreateMissingTexture(); // TODO: refactor defaults
//        string[] texturePaths = AssetManager.Instance.GetAllFilePathsOfAssetsWithExtension("png");
//        for (int i = 0; i < texturePaths.Length; i++) {
//            string texturePath = texturePaths[i];
//            Guid guid = AssetManager.Instance.GetGuidOfAsset(texturePath);
//            Load(guid, new Texture2D(texturePath));
//            Console.LogSuccess($"Loading textures ({i + 1}/{texturePaths.Length}) '{texturePath}'");
//        }
//        
//        Console.Log($"Compiling shaders...");
//        Shader.InvalidShader = InvalidShader.Create(); // TODO: refactor defaults
//        string[] shaderPaths = AssetManager.Instance.GetAllFilePathsOfAssetsWithExtension("glsl");
//        for (int i = 0; i < shaderPaths.Length; i++) {
//            Load(AssetManager.Instance.GetGuidOfAsset(shaderPaths[i]), new Shader(shaderPaths[i]));
//            Console.LogSuccess($"Compiling shaders ({i + 1}/{shaderPaths.Length}) '{shaderPaths[i]}'");
//        }
//        
//        Console.Log($"Loading Meshes");
////        float[] quadVertexData = {
////            -0.5f, 0.5f, 0.0f, 0.0f, 1.0f,   // top left
////            0.5f, 0.5f, 0.0f, 1.0f, 1.0f,    // top right
////            -0.5f, -0.5f, 0.0f, 0.0f , 0.0f, // bottom left
////
////            0.5f, 0.5f, 0.0f, 1.0f, 1.0f,   // top right
////            0.5f, -0.5f, 0.0f, 1.0f, 0.0f,  // bottom right
////            -0.5f, -0.5f, 0.0f, 0.0f, 0.0f, // bottom left
////        };
//        
//        _Vertex[] quadVertexData = {
//            new(new(-0.5f, 0.5f, 0.0f), new(0.0f, 1.0f), new()),
//            new(new(0.5f, 0.5f, 0.0f), new(1.0f, 1.0f), new()),
//            new(new(-0.5f, -0.5f, 0.0f), new(0.0f, 0.0f), new()),
//            
//            new(new(0.5f, 0.5f, 0.0f), new(1.0f, 1.0f), new()),
//            new(new(0.5f, -0.5f, 0.0f), new(1.0f, 0.0f), new()),
//            new(new(-0.5f, -0.5f, 0.0f), new(0.0f, 0.0f), new()),
//        };
//        Load(Mesh.QuadGuid, new PosUvNormalMesh(quadVertexData));
//        
//        string[] objPaths = AssetManager.Instance.GetAllFilePathsOfAssetsWithExtension("obj");
//        LoadModelsThreaded(objPaths);
//        string[] fbxPaths = AssetManager.Instance.GetAllFilePathsOfAssetsWithExtension("fbx");
//        LoadModelsThreaded(fbxPaths);
    }
    
}
