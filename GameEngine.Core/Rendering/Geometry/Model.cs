using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Assimp;
using Assimp.Configs;
using GameEngine.Core.AssetManagement;

namespace GameEngine.Core.Rendering.Geometry; 

public sealed class Model : IAsset {
    
    public static string[] Extensions { get; } = { "fbx", "obj" };
    
    public Mesh[] Meshes { get; }
    
    public Model(Mesh[] meshes) {
        Meshes = meshes;
    }
    
    public static IAsset Default(Type assetType) {
        return new Model(Array.Empty<Mesh>());
    }
    
    public static IAsset DefaultGen<T>() where T : IAsset, new() {
        return new Model(Array.Empty<Mesh>());
    }
    
    public static void LoadAssets(string[] paths) {
        LoadModelsThreaded(paths);
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
                    AssetDatabase.Load(AssetManager.Instance.GetGuidOfAsset(path), new Model(meshes));
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
        AssetDatabase.Load(AssetManager.Instance.GetGuidOfAsset(filePath), new Model(meshes));
    }
    
}
