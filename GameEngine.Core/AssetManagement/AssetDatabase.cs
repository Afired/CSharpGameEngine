using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using GameEngine.Core.Rendering.Geometry;
using GameEngine.Core.Rendering.Shaders;
using GameEngine.Core.Rendering.Textures;
using ObjLoader.Loader.Data.Elements;
using ObjLoader.Loader.Data.VertexData;
using ObjLoader.Loader.Loaders;
using Texture = GameEngine.Core.Rendering.Textures.Texture;

namespace GameEngine.Core.AssetManagement; 

public class AssetDatabase {
    
    private static readonly Dictionary<Guid, object> _assetCache = new();
    
    public static void Load(Guid guid, object asset) {
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
        if(_assetCache.TryGetValue(guid, out object? texture))
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
        float[] quadVertexData = {
            -0.5f, 0.5f, 0.0f, 0.0f, 1.0f,   // top left
            0.5f, 0.5f, 0.0f, 1.0f, 1.0f,    // top right
            -0.5f, -0.5f, 0.0f, 0.0f , 0.0f, // bottom left

            0.5f, 0.5f, 0.0f, 1.0f, 1.0f,   // top right
            0.5f, -0.5f, 0.0f, 1.0f, 0.0f,  // bottom right
            -0.5f, -0.5f, 0.0f, 0.0f, 0.0f, // bottom left
        };
        Load(Geometry.QuadGuid, new Geometry(quadVertexData));
        
        string[] paths = AssetManager.Instance.GetAllFilePathsOfAssetsWithExtension("obj");
        Thread thread = new Thread(() => LoadObjsThreaded(paths));
        thread.Start();
//        for (int i = 0; i < paths.Length; i++) {
//            Load(AssetManager.Instance.GetGuidOfAsset(paths[i]), LoadVertices(paths[i], Path.GetFileNameWithoutExtension(paths[i]).ToLower()));
//            Console.LogSuccess($"Loading model ({i + 1}/{paths.Length}) '{paths[i]}'");
//        }
        
    }
    
    private static void LoadObjsThreaded(string[] paths) {
        for (int i = 0; i < paths.Length; i++) {
            _Vertex[] vertices = LoadVertices(paths[i], Path.GetFileNameWithoutExtension(paths[i]).ToLower());
            int capturedIndex = i;
            Application.TaskQueue.Enqueue(() => {
                Load(AssetManager.Instance.GetGuidOfAsset(paths[capturedIndex]), new PosUvNormalGeometry(vertices));
            });
//            Load(AssetManager.Instance.GetGuidOfAsset(paths[i]), LoadVertices(paths[i], Path.GetFileNameWithoutExtension(paths[i]).ToLower()));
        }
    }
    
    private static _Vertex[] LoadVertices(string path, string name) {
        ObjLoaderFactory objLoaderFactory = new();
        IObjLoader loader = objLoaderFactory.Create(new MaterialNullStreamProvider());
    
        LoadResult result;
        using (FileStream fs = File.OpenRead(path)) {
            result = loader.Load(fs);
        }
        
        
        string groupName = result.Groups[0].Name;
        
        IList<Vertex> vertexList = result.Vertices;
        // IList<Texture> uvList = result.Textures;
        IList<Normal> normalList = result.Normals;
        
        
        IList<Face> faces = result.Groups[0].Faces;
        _Vertex[] vertices = new _Vertex[faces.Count * 3];
        
        for(int i = 0; i < faces.Count; i++) {
            Face face = faces[i];
            
            if(face.Count != 3) {
                Console.LogError($"Skipped face, because it had {face.Count} vertices!");
                continue;
            }
            
            {
                //vertex1
                FaceVertex faceVertex = face[0];
                Vertex position = faceVertex.VertexIndex < vertexList.Count ? vertexList[faceVertex.VertexIndex] : new Vertex(0, 0, 0);
                Normal normal = faceVertex.NormalIndex < normalList.Count ? normalList[faceVertex.NormalIndex] : new Normal(0, 0, 0);
                    
                vertices[i * 3 + 0] = new _Vertex(
                    new _Position(position.X, position.Y, position.Z),
                    new _UV(0, 0),
                    new _Normal(normal.X, normal.Y, normal.Z)
                );
            }
            {
                //vertex2
                FaceVertex faceVertex = face[1];
                Vertex position = faceVertex.VertexIndex < vertexList.Count ? vertexList[faceVertex.VertexIndex] : new Vertex(0, 0, 0);
                Normal normal = faceVertex.NormalIndex < normalList.Count ? normalList[faceVertex.NormalIndex] : new Normal(0, 0, 0);
                    
                vertices[i * 3 + 1] = new _Vertex(
                    new _Position(position.X, position.Y, position.Z),
                    new _UV(0, 0),
                    new _Normal(normal.X, normal.Y, normal.Z)
                );
            }
            {
                //vertex3
                FaceVertex faceVertex = face[2];
                Vertex position = faceVertex.VertexIndex < vertexList.Count ? vertexList[faceVertex.VertexIndex] : new Vertex(0, 0, 0);
                Normal normal = faceVertex.NormalIndex < normalList.Count ? normalList[faceVertex.NormalIndex] : new Normal(0, 0, 0);
                    
                vertices[i * 3 + 2] = new _Vertex(
                    new _Position(position.X, position.Y, position.Z),
                    new _UV(0, 0),
                    new _Normal(normal.X, normal.Y, normal.Z)
                );
            }
        }
        return vertices;
    } // PosUvNormalGeometry
    
}
