using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Assimp;
using Assimp.Configs;
using GameEngine.Core.Rendering.Geometry;
using GameEngine.Core.Rendering.Shaders;
using GameEngine.Core.Rendering.Textures;
using Mesh = GameEngine.Core.Rendering.Geometry.Mesh;
using Texture = GameEngine.Core.Rendering.Textures.Texture;

namespace GameEngine.Core.AssetManagement; 

public class AssetDatabase {
    
    private static readonly Dictionary<Guid, IAsset> _assetCache = new();
    
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
    
    public static T? Get<T>(Guid guid) where T : class {
        if(_assetCache.TryGetValue(guid, out IAsset? texture))
            return (T) texture;
        
        //TODO: Refactor handling of defaults
        if(typeof(T).IsAssignableTo(typeof(Shader)))
            return Shader.InvalidShader as T;
        if(typeof(T).IsAssignableTo(typeof(Texture)))
            return Texture2D.MissingTexture as T;
        
        return default;
    }
    
    public static void Reload() {
        UnloadAll();
        
        Console.Log($"Loading textures...");
        Texture2D.MissingTexture = Texture2D.CreateMissingTexture(); // TODO: refactor defaults
        string[] texturePaths = AssetManager.Instance.GetAllFilePathsOfAssetsWithExtension("png");
        for (int i = 0; i < texturePaths.Length; i++) {
            string texturePath = texturePaths[i];
            Guid guid = AssetManager.Instance.GetGuidOfAsset(texturePath);
            Load(guid, new Texture2D(texturePath));
            Console.LogSuccess($"Loading textures ({i + 1}/{texturePaths.Length}) '{texturePath}'");
        }
        
        Console.Log($"Compiling shaders...");
        Shader.InvalidShader = InvalidShader.Create(); // TODO: refactor defaults
        string[] shaderPaths = AssetManager.Instance.GetAllFilePathsOfAssetsWithExtension("glsl");
        for (int i = 0; i < shaderPaths.Length; i++) {
            Load(AssetManager.Instance.GetGuidOfAsset(shaderPaths[i]), new Shader(shaderPaths[i]));
            Console.LogSuccess($"Compiling shaders ({i + 1}/{shaderPaths.Length}) '{shaderPaths[i]}'");
        }
        
        Console.Log($"Loading Meshes");
//        float[] quadVertexData = {
//            -0.5f, 0.5f, 0.0f, 0.0f, 1.0f,   // top left
//            0.5f, 0.5f, 0.0f, 1.0f, 1.0f,    // top right
//            -0.5f, -0.5f, 0.0f, 0.0f , 0.0f, // bottom left
//
//            0.5f, 0.5f, 0.0f, 1.0f, 1.0f,   // top right
//            0.5f, -0.5f, 0.0f, 1.0f, 0.0f,  // bottom right
//            -0.5f, -0.5f, 0.0f, 0.0f, 0.0f, // bottom left
//        };
        
        _Vertex[] quadVertexData = {
            new(new(-0.5f, 0.5f, 0.0f), new(0.0f, 1.0f), new()),
            new(new(0.5f, 0.5f, 0.0f), new(1.0f, 1.0f), new()),
            new(new(-0.5f, -0.5f, 0.0f), new(0.0f, 0.0f), new()),
            
            new(new(0.5f, 0.5f, 0.0f), new(1.0f, 1.0f), new()),
            new(new(0.5f, -0.5f, 0.0f), new(1.0f, 0.0f), new()),
            new(new(-0.5f, -0.5f, 0.0f), new(0.0f, 0.0f), new()),
        };
        Load(Mesh.QuadGuid, new PosUvNormalMesh(quadVertexData));
        
        string[] objPaths = AssetManager.Instance.GetAllFilePathsOfAssetsWithExtension("obj");
        LoadModelsThreaded(objPaths);
        string[] fbxPaths = AssetManager.Instance.GetAllFilePathsOfAssetsWithExtension("fbx");
        LoadModelsThreaded(fbxPaths);
    }
    
    private static void LoadModels(string[] paths) {
        for(int i = 0; i < paths.Length; i++) {
            LoadModel(paths[i]);
        }
    }
    
    private static void LoadModelsThreaded(string[] paths) {
        Thread thread = new Thread(new ThreadStart(() => LoadMeshData(paths)));
        thread.Start();
        
        static void LoadMeshData(string[] paths) {
            
            foreach(string path in paths) {
                AssimpContext importer = new AssimpContext();
                importer.SetConfig(new NormalSmoothingAngleConfig(66.0f));
                Scene model = importer.ImportFile(path, PostProcessPreset.TargetRealTimeMaximumQuality);
                
                (_Vertex[] vertexData, uint[] indexData)[] meshData = new (_Vertex[], uint[])[model.MeshCount];
                
                for(int i = 0; i < model.Meshes.Count; i++) {
                    
                    List<Vector3D> posList = model.Meshes[i].Vertices;
                    List<Vector3D> normalList = model.Meshes[i].Normals;
                    List<Vector3D>[] textureCoordinateChannels = model.Meshes[i].TextureCoordinateChannels;
                    
                    _Vertex[] vertexData = new _Vertex[posList.Count];
                    
                    for(int j = 0; j < posList.Count; j++) {
                        Vector3D position = posList[j];
                        Vector3D normal = normalList[j];
                        
                        _UV uv = new _UV(0, 0);
                        if(textureCoordinateChannels.Length >= 1) {
                            List<Vector3D> uvChannel0 = textureCoordinateChannels[0];
                            if(uvChannel0.Count > j)
                                uv = new _UV(uvChannel0[j].X, uvChannel0[j].Y);
                        }
                        
                        vertexData[j] = new _Vertex(
                            new _Position(position.X, position.Y, position.Z),
                            uv,
                            new _Normal(normal.X, normal.Y, normal.Z)
                        );
                    }
                    
                    meshData[i] = (vertexData, model.Meshes[i].GetIndices().Cast<uint>().ToArray());
                }
                
                Application.TaskQueue.Enqueue(() => {
                    Mesh[] meshes = new Mesh[meshData.Length];
                    for(int i = 0; i < meshData.Length; i++) {
                        meshes[i] = new PosUvNormalMeshIndexedBuffer(meshData[i].vertexData, meshData[i].indexData);
                    }
                    Load(AssetManager.Instance.GetGuidOfAsset(path), new Model(meshes));
                });
            }
        }
        
    }
    
    private static void LoadModel(string filePath) {
        AssimpContext importer = new AssimpContext();
        importer.SetConfig(new NormalSmoothingAngleConfig(66.0f));
        Scene model = importer.ImportFile(filePath, PostProcessPreset.TargetRealTimeMaximumQuality);

        Mesh[] meshes = new Mesh[model.MeshCount];
        
        for(int i = 0; i < model.Meshes.Count; i++) {
            
            List<Vector3D> posList = model.Meshes[i].Vertices;
            List<Vector3D> normalList = model.Meshes[i].Normals;
            List<Vector3D>[] textureCoordinateChannels = model.Meshes[i].TextureCoordinateChannels;

            _Vertex[] vertexData = new _Vertex[posList.Count];
            
            for(int j = 0; j < posList.Count; j++) {
                Vector3D position = posList[j];
                Vector3D normal = normalList[j];
                
                _UV uv = new _UV(0, 0);
                if(textureCoordinateChannels.Length >= 1) {
                    List<Vector3D> uvChannel0 = textureCoordinateChannels[0];
                    if(uvChannel0.Count > j)
                        uv = new _UV(uvChannel0[j].X, uvChannel0[j].Y);
                }
                
                vertexData[j] = new _Vertex(
                    new _Position(position.X, position.Y, position.Z),
                    uv,
                    new _Normal(normal.X, normal.Y, normal.Z)
                );
            }

            uint[] indices = model.Meshes[i].GetIndices().Cast<uint>().ToArray();

            meshes[i] = new PosUvNormalMeshIndexedBuffer(vertexData, indices);
            
//            Load(AssetManager.Instance.GetGuidOfAsset(filePath), new PosUvNormalMeshIndexedBuffer(vertexData, indices));
//            continue;
//            if(i == 0)
////                Register(AssetManager.Instance.GetGuidOfAsset(filePath), new PosUvNormalGeometryIndexedBuffer(vertexData, indices));
//                Load(AssetManager.Instance.GetGuidOfAsset(filePath + "_" + i), new PosUvNormalMeshIndexedBuffer(vertexData, indices));
//            else
//                Console.LogWarning("Loading of Models with more than 1 mesh is currently not supported");
        }
        Load(AssetManager.Instance.GetGuidOfAsset(filePath), new Model(meshes));
    }
    
}
