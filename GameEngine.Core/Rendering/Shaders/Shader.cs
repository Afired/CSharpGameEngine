using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using GameEngine.Core.AssetManagement;
using GameEngine.Numerics;
using Silk.NET.OpenGL;

namespace GameEngine.Core.Rendering.Shaders;

public static class StringExtension {
    /// <summary>
    /// returns first word
    /// if no word is found returns null
    /// </summary>
    /// <param name="contents"></param>
    /// <returns></returns>
    public static string? GetFirstWord(this string contents) {
        string[] splits = contents.Split(new string[]{" ", "\r\n"}, StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
        return splits.Length > 0 ? splits[0] : default;
    }
}

public class Shader : IAsset, IDisposable {
    
    private static Shader? _invalidShader;
    public static Shader GetInvalidShader(GL gl) {
        return _invalidShader ??= new Shader(gl, InvalidShader.VERTEX_SHADER, InvalidShader.FRAGMENT_SHADER);
    }
    private readonly GL _gl;
    private uint _id;
    
    public Shader(GL gl, string vertexCode, string fragmentCode) {
        _gl = gl;
        (ShaderType shaderType, string shaderSrc)[] shaderInfo = { (ShaderType.VertexShader, vertexCode), (ShaderType.FragmentShader, fragmentCode) };
        Compile(shaderInfo);
    }
    
    public Shader(GL gl, string filePath) {
        _gl = gl;
        string contents = ReadFileWithFileStream(filePath);
        (ShaderType, string)[] shaderInfo = SplitIntoShader(contents);
        Compile(shaderInfo);
    }
    
    private void Compile((ShaderType shaderType, string shaderSrc)[] shaderInfo) {
        uint[] shaderIDs = new uint[shaderInfo.Length];
        
        for(int i = 0; i < shaderInfo.Length; i++) {
            
            ShaderType type = shaderInfo[i].shaderType;
            string src = shaderInfo[i].shaderSrc;
            
            shaderIDs[i] = _gl.CreateShader(type);
            _gl.ShaderSource(shaderIDs[i], src);
            _gl.CompileShader(shaderIDs[i]);
            
            //int[] status = GL.GetShaderiv(shaderIDs[i], GL.GL_COMPILE_STATUS, 1);
            //if(status[0] == 0) {
            //    string error = GL.GetShaderInfoLog(shaderIDs[i]);
            //    throw new Exception($"Shader failed to compile {error}");
            //}
            
        }
        
        _id = _gl.CreateProgram();
        for(int i = 0; i < shaderIDs.Length; i++) {
            _gl.AttachShader(_id, shaderIDs[i]);
        }
        _gl.LinkProgram(_id);
        
        // Delete Shaders
        for(int i = 0; i < shaderIDs.Length; i++) {
            _gl.DetachShader(_id, shaderIDs[i]);
            _gl.DeleteShader(shaderIDs[i]);
        }
    }

    private static string ReadFileWithFileStream(string filePath) {
        using FileStream fs = File.OpenRead(filePath);
        byte[] buffer = new byte[fs.Length];
        fs.Read(buffer, 0, (int) fs.Length);
        fs.Dispose();
        return Encoding.Default.GetString(buffer);
    }
    
    private static string ReadFileWithStreamReader(string filePath) {
        using StreamReader sr = File.OpenText(filePath);
        string result = sr.ReadToEnd();
        sr.Dispose();
        return result;
    }
    
    private static (ShaderType shaderType, string shaderSrc)[] SplitIntoShader(string contents) {
        const string SHADER_TYPE_KEYWORD = "#type";
        string[] shaderSrcs = contents.Split(SHADER_TYPE_KEYWORD, StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
        (ShaderType shaderType, string shaderSrc)[] result = new (ShaderType shaderType, string shaderSrc)[shaderSrcs.Length];
        for(int i = 0; i < shaderSrcs.Length; i++) {
            string keyword = shaderSrcs[i].GetFirstWord();
            Debug.Assert(!string.IsNullOrEmpty(keyword));

            result[i].shaderType = ShaderTypeFromString(keyword);
            result[i].shaderSrc = shaderSrcs[i].Replace(keyword, "");
        }
        return result;
    }
    
    private static ShaderType ShaderTypeFromString(string type) => type switch {
        "vertex" => ShaderType.VertexShader,
        "fragment" => ShaderType.FragmentShader,
        "pixel" => ShaderType.FragmentShader,
        _ => throw new Exception($"unsupported shader type: '{type}'")
    };
    
    public void Use() {
        _gl.UseProgram(_id);
    }
    
    public void SetMat4x4(string uniformName, Matrix<float> matrix) {
        int location = _gl.GetUniformLocation(_id, uniformName);
        _gl.UniformMatrix4(location, 1, false, Matrix<float>.ToFloatArray(matrix));
    }
    
    public void SetInt(string uniformName, int value) {
        int location = _gl.GetUniformLocation(_id, uniformName);
        _gl.Uniform1(location, value);
    }
    
    public void SetFloat(string uniformName, float value) {
        int location = _gl.GetUniformLocation(_id, uniformName);
        _gl.Uniform1(location, value);
    }
    
    public void SetVector3(string uniformName, Vec3<float> vec3) {
        int location = _gl.GetUniformLocation(_id, uniformName);
        _gl.Uniform3(location, new System.Numerics.Vector3(vec3.X, vec3.Y, vec3.Z));
    }
    
    public int GetAttributeLocation(ReadOnlySpan<byte> attributeName) {
        return _gl.GetAttribLocation(_id, attributeName);
    }
    
    public int GetAttributeLocation(ReadOnlySpan<char> attributeName) {
        Span<byte> bytes = default;
        Encoding.UTF8.GetBytes(attributeName, bytes);
        return _gl.GetAttribLocation(_id, bytes);
    }
    
    public int GetUniformLocation(ReadOnlySpan<byte> attributeName) {
        return _gl.GetUniformLocation(_id, attributeName);
    }
    
    public int GetUniformLocation(ReadOnlySpan<char> attributeName) {
        Span<byte> bytes = default;
        Encoding.UTF8.GetBytes(attributeName, bytes);
        return _gl.GetUniformLocation(_id, bytes);
    }
    
    public void Dispose() {
        _gl.DeleteShader(_id);
    }
}
