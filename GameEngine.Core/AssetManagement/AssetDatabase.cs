using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using GameEngine.Core.Rendering.Geometry;

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
    private static readonly List<IAssetImporter> _assetImporterCache = new();
    
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
        _assetImporterCache.Clear();
    }
    
    public static T? Get<T>(Guid guid) where T : class, IAsset {
        if(_assetCache.TryGetValue(guid, out IAsset? asset)) {
            if(asset is T t)
                return t;
            
            //TODO: return registered default asset instead
            return default(T);
        }
        
        //TODO: Refactor handling of defaults
//        IAsset? result = _defaultAssets.FirstOrDefault(asset => asset.GetType() == typeof(T));
//        result ??= _defaultAssets.FirstOrDefault(asset => asset.GetType().IsAssignableTo(typeof(T)));
        
        return default(T);
    }
    
    public static void Reload() {
        UnloadAll();
        
        // instantiate assetImporters
        IEnumerable<Type> assetImporterTypes = Application.GetExternalAssembliesStatic.Append(Assembly.GetAssembly(typeof(Application))!).
            SelectMany(assembly => ReflectionHelper.GetDerivedTypes(typeof(AssetImporter<>), assembly));
        foreach(Type assetImporterType in assetImporterTypes) {
            IAssetImporter? assetImporter = (IAssetImporter?) Activator.CreateInstance(assetImporterType);
            if(assetImporter is null) {
                Console.LogWarning($"Failed to instantiate asset importer of type {assetImporterType.FullName}");
                continue;
            }
            _assetImporterCache.Add(assetImporter);
        }
        
        //TODO: load default assets?
        
        // load assets with assetImporters
        foreach(IAssetImporter assetImporter in _assetImporterCache) {
            foreach(string extension in assetImporter.GetExtensions()) {
                foreach(string path in AssetManager.Instance.GetAllFilePathsOfAssetsWithExtension(extension)) {
                    
                    IAsset? asset = assetImporter.ImportInternal(path);
                    if(asset is null)
                        continue;
                    
                    Guid guid = AssetManager.Instance.GetGuidOfAsset(path);
                    AssetDatabase.Load(guid, asset);
                    
                }
            }
        }
        
        //TODO: refactor default asset init
        Load(Mesh.QuadGuid, new Model(new Mesh[] { Mesh.CreateDefault() }));
        
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
