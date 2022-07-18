// using System.Collections.Generic;
// using System.Linq;
// using System.Numerics;
// using Assimp;
// using ObjLoader.Loader.Data.VertexData;
// using Silk.NET.OpenGL;
//
// namespace GameEngine.Core.Rendering.Geometry; 
//
// public class PosGeometry : Geometry {
//
//     public PosGeometry(List<Vector3D> vertexData) : base() {
//         VertexCount = vertexData.Count;
//         InitializeGeometry(vertexData);
//     }
//     
//     private void InitializeGeometry(List<Vector3D> vertexData) {
//         
//         Vao = Gl.GenVertexArray();
//         Vbo = Gl.GenBuffer();
//         
//         Gl.BindVertexArray(Vao);
//         Gl.BindBuffer(BufferTargetARB.ArrayBuffer, Vbo);
//         
//         
//         unsafe {
//
//             // IList<Vector3> vertecies = null;
//             // fixed(byte* v = &vertecies[0]) {
//             //     Gl.BufferData(BufferTargetARB.ArrayBuffer, (nuint) (sizeof(_Vertex) * vertexData.Count), v, BufferUsageARB.StaticDraw);
//             // }
//             
//             fixed(void* v = &vertexData[0]) {
//                 Gl.BufferData(BufferTargetARB.ArrayBuffer, (nuint) (sizeof(Vector3) * VertexCount), v, BufferUsageARB.StaticDraw);
//             }
//             
//             // position:xyz
//             //                     index in shader, how many elements, element type, should be normalized, size, offset from start
//             Gl.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), (void*) (0 * sizeof(float)));
//             Gl.EnableVertexAttribArray(0);
//             
//             // // texture:uv
//             // Gl.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, 8 * sizeof(float), (void*) (3 * sizeof(float)));
//             // Gl.EnableVertexAttribArray(1);
//             //
//             // // normal:xyz
//             // Gl.VertexAttribPointer(2, 3, VertexAttribPointerType.Float, true, 8 * sizeof(float), (void*) (5 * sizeof(float)));
//             // Gl.EnableVertexAttribArray(2);
//             
//             Gl.BindBuffer(BufferTargetARB.ArrayBuffer, 0);
//             Gl.BindVertexArray(0);
//         }
//         
//     }
//     
// }