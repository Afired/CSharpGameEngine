using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;
using GameEngine.Rendering;
using Silk.NET.GLFW;
using Silk.NET.OpenGL;
using ErrorCode = Silk.NET.OpenGL.ErrorCode;

namespace Dear_ImGui_Sample
{
    static class Util {
        private static GL GL = RenderingEngine.Gl;
        
        [Pure]
        public static float Clamp(float value, float min, float max)
        {
            return value < min ? min : value > max ? max : value;
        }

        [Conditional("DEBUG")]
        public static void CheckGLError(string title)
        {
            var error = GL.GetError();
            if (error != (int) ErrorCode.NoError)
            {
                Debug.Print($"{title}: {error}");
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void LabelObject(ObjectIdentifier objectIdentifier, int glObject, string name)
        {
            GL.ObjectLabel(objectIdentifier, (uint) glObject, (uint) name.Length, name);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CreateTexture(TextureTarget target, string Name, out uint Texture)
        {
            //GL.CreateTextures(target, 1, out Texture);
            Texture = GL.GenTexture();
            LabelObject(ObjectIdentifier.Texture, (int) Texture, $"Texture: {Name}");
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CreateProgram(string Name, out uint Program) {
            Program = GL.CreateProgram();
            LabelObject(ObjectIdentifier.Program, (int) Program, $"Program: {Name}");
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CreateShader(ShaderType type, string Name, out uint Shader)
        {
            Shader = GL.CreateShader(type);
            LabelObject(ObjectIdentifier.Shader, (int) Shader, $"Shader: {type}: {Name}");
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CreateBuffer(string Name, out uint Buffer) {
            Buffer = GL.GenBuffer();
            LabelObject(ObjectIdentifier.Buffer, (int) Buffer, $"Buffer: {Name}");
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CreateVertexBuffer(string Name, out uint Buffer) => CreateBuffer($"VBO: {Name}", out Buffer);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CreateElementBuffer(string Name, out uint Buffer) => CreateBuffer($"EBO: {Name}", out Buffer);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CreateVertexArray(string Name, out uint VAO) {
            VAO = GL.GenVertexArray();
            LabelObject(ObjectIdentifier.VertexArray, (int) VAO, $"VAO: {Name}");
        }
    }
}