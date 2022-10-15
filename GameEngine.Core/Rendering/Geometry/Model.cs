using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Assimp;
using Assimp.Configs;
using GameEngine.Core.AssetManagement;

namespace GameEngine.Core.Rendering.Geometry; 

public class Model : IAsset {
    
    public Mesh[] Meshes { get; }
    
    public static Model Empty { get; }
    
    static Model() {
        Empty = CreateEmpty();
    }
    
    private static Model CreateEmpty() {
        return new Model(Array.Empty<Mesh>());
    }
    
    public Model(Mesh[] meshes) {
        Meshes = meshes;
    }
    
    public Model(string path) {
        AssimpContext importer = new AssimpContext();
        importer.SetConfig(new NormalSmoothingAngleConfig(66.0f));
        Scene model = importer.ImportFile(path, PostProcessPreset.TargetRealTimeMaximumQuality);
        
        Mesh[] meshes = new Mesh[model.MeshCount];
        
        for(int i = 0; i < model.Meshes.Count; i++) {
            
            List<Vector3D> posList = model.Meshes[i].Vertices;
            List<Vector3D> normalList = model.Meshes[i].Normals;
            List<Vector3D>[] textureCoordinateChannels = model.Meshes[i].TextureCoordinateChannels;
            
            Vertex[] vertexData = new Vertex[posList.Count];
            
            for(int j = 0; j < posList.Count; j++) {
                Vector3D position = posList[j];
                Vector3D normal = normalList[j];
                
                Uv uv = new Uv(0, 0);
                if(textureCoordinateChannels.Length >= 1) {
                    List<Vector3D> uvChannel0 = textureCoordinateChannels[0];
                    if(uvChannel0.Count > j)
                        uv = new Uv(uvChannel0[j].X, uvChannel0[j].Y);
                }
                
                vertexData[j] = new Vertex(
                    new Position(position.X, position.Y, position.Z),
                    uv,
                    new Normal(normal.X, normal.Y, normal.Z)
                );
            }
            
            uint[] indices = model.Meshes[i].GetIndices().Cast<uint>().ToArray();
            
            meshes[i] = new Mesh(vertexData, indices);
        }
        Meshes = meshes;
    }
    
    public static void LoadModelsThreaded(string[] paths) {
        Thread thread = new Thread(new ThreadStart(() => LoadMeshData(paths)));
        thread.Start();
        
        static void LoadMeshData(string[] paths) {
            
            foreach(string path in paths) {
                AssimpContext importer = new AssimpContext();
                importer.SetConfig(new NormalSmoothingAngleConfig(66.0f));
                Scene model = importer.ImportFile(path, PostProcessPreset.TargetRealTimeMaximumQuality);
                
                (Vertex[] vertexData, uint[] indexData)[] meshData = new (Vertex[], uint[])[model.MeshCount];
                
                for(int i = 0; i < model.Meshes.Count; i++) {
                    
                    List<Vector3D> posList = model.Meshes[i].Vertices;
                    List<Vector3D> normalList = model.Meshes[i].Normals;
                    List<Vector3D>[] textureCoordinateChannels = model.Meshes[i].TextureCoordinateChannels;
                    
                    Vertex[] vertexData = new Vertex[posList.Count];
                    
                    for(int j = 0; j < posList.Count; j++) {
                        Vector3D position = posList[j];
                        Vector3D normal = normalList[j];
                        
                        Uv uv = new Uv(0, 0);
                        if(textureCoordinateChannels.Length >= 1) {
                            List<Vector3D> uvChannel0 = textureCoordinateChannels[0];
                            if(uvChannel0.Count > j)
                                uv = new Uv(uvChannel0[j].X, uvChannel0[j].Y);
                        }
                        
                        vertexData[j] = new Vertex(
                            new Position(position.X, position.Y, position.Z),
                            uv,
                            new Normal(normal.X, normal.Y, normal.Z)
                        );
                    }
                    
                    meshData[i] = (vertexData, model.Meshes[i].GetIndices().Cast<uint>().ToArray());
                }
                
                Application.TaskQueue.Enqueue(() => {
                    Mesh[] meshes = new Mesh[meshData.Length];
                    for(int i = 0; i < meshData.Length; i++) {
                        meshes[i] = new Mesh(meshData[i].vertexData, meshData[i].indexData);
                    }
                    AssetDatabase.Load(AssetManager.Instance.GetGuidOfAsset(path), new Model(meshes));
                });
            }
        }
        
    }
    
    public static void LoadModel(string filePath) {
        AssimpContext importer = new AssimpContext();
        importer.SetConfig(new NormalSmoothingAngleConfig(66.0f));
        Scene model = importer.ImportFile(filePath, PostProcessPreset.TargetRealTimeMaximumQuality);

        Mesh[] meshes = new Mesh[model.MeshCount];
        
        for(int i = 0; i < model.Meshes.Count; i++) {
            
            List<Vector3D> posList = model.Meshes[i].Vertices;
            List<Vector3D> normalList = model.Meshes[i].Normals;
            List<Vector3D>[] textureCoordinateChannels = model.Meshes[i].TextureCoordinateChannels;

            Vertex[] vertexData = new Vertex[posList.Count];
            
            for(int j = 0; j < posList.Count; j++) {
                Vector3D position = posList[j];
                Vector3D normal = normalList[j];
                
                Uv uv = new Uv(0, 0);
                if(textureCoordinateChannels.Length >= 1) {
                    List<Vector3D> uvChannel0 = textureCoordinateChannels[0];
                    if(uvChannel0.Count > j)
                        uv = new Uv(uvChannel0[j].X, uvChannel0[j].Y);
                }
                
                vertexData[j] = new Vertex(
                    new Position(position.X, position.Y, position.Z),
                    uv,
                    new Normal(normal.X, normal.Y, normal.Z)
                );
            }

            uint[] indices = model.Meshes[i].GetIndices().Cast<uint>().ToArray();

            meshes[i] = new Mesh(vertexData, indices);
        }
        AssetDatabase.Load(AssetManager.Instance.GetGuidOfAsset(filePath), new Model(meshes));
    }
    
    public void Dispose() {
        //TODO: Dispose
    }
    
}
