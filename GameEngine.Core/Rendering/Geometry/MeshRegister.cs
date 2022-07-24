using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Assimp;
using Assimp.Configs;
using GameEngine.Core.AssetManagement;
using GameEngine.Core.Debugging;
using GameEngine.Core.Guard;
using GameEngine.Core.Rendering.Textures;
using ObjLoader.Loader.Data.Elements;
using ObjLoader.Loader.Data.VertexData;
using ObjLoader.Loader.Loaders;
using Silk.NET.OpenGL;
using Face = ObjLoader.Loader.Data.Elements.Face;

namespace GameEngine.Core.Rendering.Geometry;

public static class MeshRegister {
    
    private static readonly ConcurrentDictionary<string, Geometry> _meshRegister = new();
    
    public static void Register(string name, Geometry shader) {
        name = name.ToLower();
        Throw.If(_meshRegister.ContainsKey(name), "duplicate geometry");
        // _meshRegister.Add(name, shader);
        _meshRegister.TryAdd(name, shader);
    }
    
    public static Geometry? Get(string name) {
        name = name.ToLower();
        if(_meshRegister.TryGetValue(name, out Geometry geometry))
            return geometry;
        Console.LogWarning($"Geometry not found '{name}'");
        return null;
    }
    
    public static void Reload() {
        _meshRegister.Clear();
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
        
        string[] paths = AssetManager.Instance.GetAllFilePathsOfAssetsWithExtension("obj");
        for (int i = 0; i < paths.Length; i++) {
            LoadObjFaces(paths[i], Path.GetFileNameWithoutExtension(paths[i]).ToLower());
            Console.LogSuccess($"Loading model ({i + 1}/{paths.Length}) '{paths[i]}'");
        }
        
        // LoadObjFaces(@"D:\Dev\C#\CSharpGameEngine\ExampleProject\Assets\Models\car_tri.obj", "car_tri");
        // LoadObjFaces(@"D:\Dev\C#\CSharpGameEngine\ExampleProject\Assets\Models\higokumaru-honkai-impact-3rd_tri.obj", "honkai girl_tri");
        // LoadObjFaces(@"D:\Dev\C#\CSharpGameEngine\ExampleProject\Assets\Models\staff.obj", "staff");
        // LoadObjFaces(@"D:\Dev\C#\CSharpGameEngine\ExampleProject\Assets\Models\temple.obj", "church");
        // LoadModelUsingAssimp(@"D:\Dev\C#\CSharpGameEngine\ExampleProject\Assets\Models\honkai girl.gltf", "honkai girl");
        // RegisterTestModelWithEBOs();
    }
    
    // private static void LoadObj(string path, string name) {
    //     ObjLoaderFactory objLoaderFactory = new();
    //     IObjLoader loader = objLoaderFactory.Create(new MaterialNullStreamProvider());
    //
    //     LoadResult result;
    //     using (FileStream fs = File.OpenRead(path)) {
    //         result = loader.Load(fs);
    //     }
    //     
    //     IList<Vertex> vertexList = result.Vertices;
    //     // IList<Texture> uvList = result.Textures;
    //     // IList<Normal> normalsList = result.Normals;
    //     
    //     
    //     
    //     Face face = result.Groups[0].Faces[0];
    //     int faceCount = face.Count;
    //     
    //     float[] vertices = new float[vertexList.Count * 5];
    //     for(int i = 0; i < vertexList.Count; i++) {
    //         vertices[i * 5 + 0] = vertexList[i].X;
    //         vertices[i * 5 + 1] = vertexList[i].Y;
    //         vertices[i * 5 + 2] = vertexList[i].Z;
    //         // vertices[i * 5 + 3] = uvList[i].X;
    //         // vertices[i * 5 + 4] = uvList[i].Y;
    //         
    //         vertices[i * 5 + 3] = 0;
    //         vertices[i * 5 + 4] = 0;
    //     }
    //     Register(name, new Geometry(vertices));
    // }
    
    private static void LoadObjFaces(string path, string name) {
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
        
        Register(name, new PosUvNormalGeometry(vertices));
    }

    // private static void LoadGltf(string path, string name) {
    //     ModelRoot modelRoot = ModelRoot.Load(path, new ReadSettings() {
    //         Validation = ValidationMode.TryFix
    //     });
    //
    //     int i = 0;
    //     foreach(Node node in modelRoot.DefaultScene.VisualChildren) {
    //         foreach(MeshPrimitive meshPrimitive in node.Mesh.Primitives) {
    //             Register(name + i, new PosGeometry(meshPrimitive.IndexAccessor));
    //             i++;
    //         }
    //     }
    //     
    //     
    //     // IReadOnlyList<Mesh>? meshes = modelRoot.CreateMeshes();
    //     // IReadOnlyList<MeshPrimitive>? primitives = meshes[0].Primitives;
    //     // int logicalIndex = primitives[0].LogicalIndex;
    //
    //     // Accessor? accessor = modelRoot.LogicalAccessors[0];
    //     // var count = modelRoot.LogicalAccessors[0].Count;
    //     // var bufferview = modelRoot.LogicalAccessors[0].SourceBufferView;
    //     // var vector3Array = modelRoot.LogicalAccessors[0].AsVector3Array();
    //
    //     // Register(name + 0, new PosGeometry(modelRoot.LogicalAccessors[0]));
    //     // Register(name + 1, new PosGeometry(modelRoot.LogicalAccessors[1]));
    //     // Register(name + 4, new PosGeometry(modelRoot.LogicalAccessors[4]));
    //     // Register(name + 8, new PosGeometry(modelRoot.LogicalAccessors[8]));
    //     // Register(name + 12, new PosGeometry(modelRoot.LogicalAccessors[12]));
    //     // Register(name + 16, new PosGeometry(modelRoot.LogicalAccessors[16]));
    // }

    private static void LoadModelUsingAssimp(string filePath, string name) {
        AssimpContext importer = new AssimpContext();
        importer.SetConfig(new NormalSmoothingAngleConfig(66.0f));
        Scene model = importer.ImportFile(filePath, PostProcessPreset.TargetRealTimeMaximumQuality);
        
        for(int i = 0; i < model.Meshes.Count; i++) {
            
            List<Vector3D> posList = model.Meshes[i].Vertices;
            List<Vector3D> normalList = model.Meshes[i].Normals;
            
            _Vertex[] vertexData = new _Vertex[posList.Count];
            
            for(int j = 0; j < posList.Count; j++) {
                Vector3D position = posList[j];
                Vector3D normal = normalList[j];
                
                vertexData[j] = new _Vertex(
                    new _Position(position.X, position.Y, position.Z),
                    new _UV(0, 0),
                    new _Normal(normal.X, normal.Y, normal.Z)
                );
            }

            uint[] indices = model.Meshes[i].GetIndices().Cast<uint>().ToArray();
            Register(name + i, new PosUvNormalGeometryIndexedBuffer(vertexData, indices));
        }
        
    }

    private static void RegisterTestModelWithEBOs() {
        
        float[] vertices = {
            0.5f,  0.5f, 0.0f,  // top right
            0.5f, -0.5f, 0.0f,  // bottom right
            -0.5f, -0.5f, 0.0f,  // bottom left
            -0.5f,  0.5f, 0.0f, // top left 
        };
        uint[] indices = {  // note that we start from 0!
            0, 1, 3,   // first triangle
            1, 2, 3, // second triangle
        };

        _Vertex[] vertexData = {
            new(
                new _Position(0.5f, 0.5f, 0.0f),
                new _UV(1, 1),
                new _Normal(0, 1, 0)
            ),
            new(
                new _Position(0.5f, -0.5f, 0.0f),
                new _UV(0, 1),
                new _Normal(0, 1, 0)
            ),
            new(
                new _Position(-0.5f, -0.5f, 0.0f),
                new _UV(1, 0),
                new _Normal(0, 1, 0)
            ),
            new(
                new _Position(-0.5f, 0.5f, 0.0f),
                new _UV(0, 0),
                new _Normal(0, 1, 0)
            ),
        };
        Register("EBO_Test_Quad", new PosUvNormalGeometryIndexedBuffer(vertexData, indices));
    }
    
}
