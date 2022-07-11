using System.Collections.Generic;
using System.IO;
using System.Text;
using GameEngine.Core.Debugging;
using GameEngine.Core.Guard;
using glTFLoader.Schema;
using ObjLoader.Loader.Data.Elements;
using ObjLoader.Loader.Data.VertexData;
using ObjLoader.Loader.Loaders;

namespace GameEngine.Core.Rendering.Geometry;

public static class GeometryRegister {
    
    internal static Dictionary<string, Geometry> _geometryRegister;
    
    
    static GeometryRegister() {
        _geometryRegister = new Dictionary<string, Geometry>();
    }

    public static void Register(string name, Geometry shader) {
        name = name.ToLower();
        Throw.If(_geometryRegister.ContainsKey(name), "duplicate geometry");
        _geometryRegister.Add(name, shader);
    }

    public static Geometry Get(string name) {
        if(name is null) {
            Console.LogWarning($"Geometry not found 'null'");
            return null;
        }
        name = name.ToLower();
        if(_geometryRegister.TryGetValue(name, out Geometry shader))
            return shader;
        Console.LogWarning($"Geometry not found '{name}'");
        return null;
    }
    
    public static void Load() {
        Console.Log($"Initializing geometry...");
        float[] quadVertexData = {
            -0.5f, 0.5f, 0.0f, 0.0f, 1.0f,   // top left
            0.5f, 0.5f, 0.0f, 1.0f, 1.0f,    // top right
            -0.5f, -0.5f, 0.0f, 0.0f , 0.0f, // bottom left

            0.5f, 0.5f, 0.0f, 1.0f, 1.0f,   // top right
            0.5f, -0.5f, 0.0f, 1.0f, 0.0f,  // bottom right
            -0.5f, -0.5f, 0.0f, 0.0f, 0.0f, // bottom left
        };
        Register("Quad", new Geometry(quadVertexData));
        
        LoadObj(@"D:\Dev\C#\CSharpGameEngine\ExampleProject\Assets\Models\car.obj", "car");
        LoadObjFaces(@"D:\Dev\C#\CSharpGameEngine\ExampleProject\Assets\Models\car_tri.obj", "car_tri");
        LoadObj(@"D:\Dev\C#\CSharpGameEngine\ExampleProject\Assets\Models\higokumaru-honkai-impact-3rd.obj", "honkai girl");
        LoadObjFaces(@"D:\Dev\C#\CSharpGameEngine\ExampleProject\Assets\Models\higokumaru-honkai-impact-3rd_tri.obj", "honkai girl_tri");
        // LoadObj(@"D:\Dev\C#\CSharpGameEngine\ExampleProject\Assets\Models\demon_girl.obj", "demon girl");
        // LoadObjFaces(@"D:\Dev\C#\CSharpGameEngine\ExampleProject\Assets\Models\demon_girl.obj", "demon girl_tri");
    }
    
    private static void LoadObj(string path, string name) {
        ObjLoaderFactory objLoaderFactory = new();
        IObjLoader loader = objLoaderFactory.Create(new MaterialNullStreamProvider());
    
        LoadResult result;
        using (FileStream fs = File.OpenRead(path)) {
            result = loader.Load(fs);
        }
        
        IList<Vertex> vertexList = result.Vertices;
        // IList<Texture> uvList = result.Textures;
        // IList<Normal> normalsList = result.Normals;
        
        
        
        Face face = result.Groups[0].Faces[0];
        int faceCount = face.Count;
        
        float[] vertices = new float[vertexList.Count * 5];
        for(int i = 0; i < vertexList.Count; i++) {
            vertices[i * 5 + 0] = vertexList[i].X;
            vertices[i * 5 + 1] = vertexList[i].Y;
            vertices[i * 5 + 2] = vertexList[i].Z;
            // vertices[i * 5 + 3] = uvList[i].X;
            // vertices[i * 5 + 4] = uvList[i].Y;
            
            vertices[i * 5 + 3] = 0;
            vertices[i * 5 + 4] = 0;
        }
        Register(name, new Geometry(vertices));
    }
    
    private static void LoadObjFaces(string path, string name) {
        ObjLoaderFactory objLoaderFactory = new();
        IObjLoader loader = objLoaderFactory.Create(new MaterialNullStreamProvider());
    
        LoadResult result;
        using (FileStream fs = File.OpenRead(path)) {
            result = loader.Load(fs);
        }
        
        IList<Vertex> vertexList = result.Vertices;
        // IList<Texture> uvList = result.Textures;
        // IList<Normal> normalsList = result.Normals;
        
        
        IList<Face> faces = result.Groups[0].Faces;
        string groupName = result.Groups[0].Name;
        
        float[] vertices = new float[faces.Count * 3 * 5];
        
        for(int i = 0; i < faces.Count; i++) {
            Face face = faces[i];
            
            if(face.Count != 3) {
                Console.LogError($"Skipped face, because it had {face.Count} vertices!");
                continue;
            }
            
            {
                //vertex1
                FaceVertex faceVertex = face[0];
                if(faceVertex.VertexIndex < vertexList.Count) {
                    Vertex vertex = vertexList[faceVertex.VertexIndex];
                
                    vertices[(i * 3 + 0) * 5 + 0] = vertex.X;
                    vertices[(i * 3 + 0) * 5 + 1] = vertex.Y;
                    vertices[(i * 3 + 0) * 5 + 2] = vertex.Z;
                    // vertices[(i * 3 + 0) * 5 + 3] = 0;
                    // vertices[(i * 3 + 0) * 5 + 4] = 0;
                } else {
                    Console.LogError($"Vertex index was out of bounds!");
                    vertices[(i * 3 + 0) * 5 + 0] = 100;
                    vertices[(i * 3 + 0) * 5 + 1] = 100;
                    vertices[(i * 3 + 0) * 5 + 2] = 100;
                }
            }
            {
                //vertex2
                FaceVertex faceVertex = face[1];
                if(faceVertex.VertexIndex < vertexList.Count) {
                    Vertex vertex = vertexList[faceVertex.VertexIndex];

                    vertices[(i * 3 + 1) * 5 + 0] = vertex.X;
                    vertices[(i * 3 + 1) * 5 + 1] = vertex.Y;
                    vertices[(i * 3 + 1) * 5 + 2] = vertex.Z;
                    // vertices[(i * 3 + 1) * 5 + 3] = 0;
                    // vertices[(i * 3 + 1) * 5 + 4] = 0;}
                } else {
                    Console.LogError($"Vertex index was out of bounds!");
                    vertices[(i * 3 + 0) * 5 + 0] = 100;
                    vertices[(i * 3 + 0) * 5 + 1] = 100;
                    vertices[(i * 3 + 0) * 5 + 2] = 100;
                }
            }
            {
                //vertex3
                FaceVertex faceVertex = face[2];
                if(faceVertex.VertexIndex < vertexList.Count) {
                    Vertex vertex = vertexList[faceVertex.VertexIndex];

                    vertices[(i * 3 + 2) * 5 + 0] = vertex.X;
                    vertices[(i * 3 + 2) * 5 + 1] = vertex.Y;
                    vertices[(i * 3 + 2) * 5 + 2] = vertex.Z;
                    // vertices[(i * 3 + 2) * 5 + 3] = 0;
                    // vertices[(i * 3 + 2) * 5 + 4] = 0;
                } else {
                    Console.LogError($"Vertex index was out of bounds!");
                    vertices[(i * 3 + 0) * 5 + 0] = 100;
                    vertices[(i * 3 + 0) * 5 + 1] = 100;
                    vertices[(i * 3 + 0) * 5 + 2] = 100;
                }
            }
        }
        
        Register(name, new Geometry(vertices));
    }
    
}
