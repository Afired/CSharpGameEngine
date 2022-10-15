using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using GameEngine.Core.Rendering.Geometry;

namespace GameEngine.Core.AssetManagement;

public static class AssetDatabase {
    
    private static readonly Dictionary<Guid, IAsset> _assetCache = new();
    private static readonly List<AssetImporter> _assetImporterCache = new();
    
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
        if(_assetCache.TryGetValue(guid, out IAsset? asset))
            return (T) asset;
        return default(T);
    }
    
    public static void Reload() {
        UnloadAll();
        
        // instantiate assetImporters
        IEnumerable<Type> assetImporterTypes = Application.GetExternalAssembliesStatic.Append(Assembly.GetAssembly(typeof(Application))!)
            .SelectMany(assembly => typeof(AssetImporter<>).GetDerivedTypes(assembly));
        foreach(Type assetImporterType in assetImporterTypes) {
            AssetImporter? assetImporter = (AssetImporter?) Activator.CreateInstance(assetImporterType);
            if(assetImporter is null) {
                Console.LogWarning($"Failed to instantiate asset importer of type {assetImporterType.FullName}");
                continue;
            }
            _assetImporterCache.Add(assetImporter);
        }
        
        //TODO: load default assets?
        
        // load assets with assetImporters
        foreach(AssetImporter assetImporter in _assetImporterCache) {
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
        Load(Mesh.QuadGuid, Mesh.CreateQuad());
    }
    
}
