using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using GameEngine.Core.AssetManagement;
using GameEngine.Core.Numerics;
using GlmNet;
using Silk.NET.OpenGL;

namespace GameEngine.Core.Rendering.Shaders;

public static class StringExtension {
    /// <summary>
    /// returns first word
    /// if no word is found returns null
    /// </summary>
    /// <param name="contents"></param>
    /// <returns></returns>
    public static string GetFirstWord(this string contents) {
        string[] splits = contents.Split(new string[]{" ", "\r\n"}, StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
        return splits.Length > 0 ? splits[0] : default;
    }
}

public class Shader : IAsset {
    
    public static IAsset Default { get; }
    
    static Shader() {
        Default = InvalidShader.Create();
    }
    
    private uint _programID;
    
    public Shader(string vertexCode, string fragmentCode) {
        (ShaderType shaderType, string shaderSrc)[] shaderInfo = { (ShaderType.VertexShader, vertexCode), (ShaderType.FragmentShader, fragmentCode) };
        Compile(shaderInfo);
    }
    
    public Shader(string filePath) {
        string contents = ReadFileWithFileStream(filePath);
        (ShaderType, string)[] shaderInfo = SplitIntoShader(contents);
        Compile(shaderInfo);
    }
    
    private void Compile((ShaderType shaderType, string shaderSrc)[] shaderInfo) {
        uint[] shaderIDs = new uint[shaderInfo.Length];
        
        for(int i = 0; i < shaderInfo.Length; i++) {
            
            ShaderType type = shaderInfo[i].shaderType;
            string src = shaderInfo[i].shaderSrc;
            
            shaderIDs[i] = Gl.CreateShader(type);
            Gl.ShaderSource(shaderIDs[i], src);
            Gl.CompileShader(shaderIDs[i]);
            
            //int[] status = GL.GetShaderiv(shaderIDs[i], GL.GL_COMPILE_STATUS, 1);
            //if(status[0] == 0) {
            //    string error = GL.GetShaderInfoLog(shaderIDs[i]);
            //    throw new Exception($"Shader failed to compile {error}");
            //}
            
        }
        
        _programID = Gl.CreateProgram();
        for(int i = 0; i < shaderIDs.Length; i++) {
            Gl.AttachShader(_programID, shaderIDs[i]);
        }
        Gl.LinkProgram(_programID);
        
        // Delete Shaders
        for(int i = 0; i < shaderIDs.Length; i++) {
            Gl.DetachShader(_programID, shaderIDs[i]);
            Gl.DeleteShader(shaderIDs[i]);
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
        Gl.UseProgram(_programID);
    }
    
    public void SetMatrix4x4(string uniformName, Matrix4x4 mat) {
        int location = Gl.GetUniformLocation(_programID, uniformName);
        Gl.UniformMatrix4(location, 1, false, mat.ToArray());
    }
    
    public void GLM_SetMat(string uniformName, mat4 mat4) {
        int location = Gl.GetUniformLocation(_programID, uniformName);
        Gl.UniformMatrix4(location, 1, false, mat4.to_array());
    }
    
    public void SetInt(string uniformName, int value) {
        int location = Gl.GetUniformLocation(_programID, uniformName);
        Gl.Uniform1(location, value);
    }
    
    public void SetFloat(string uniformName, float value) {
        int location = Gl.GetUniformLocation(_programID, uniformName);
        Gl.Uniform1(location, value);
    }
    
    public void SetVector3(string uniformName, Vector3 vector3) {
        int location = Gl.GetUniformLocation(_programID, uniformName);
        var sysVector3 = new System.Numerics.Vector3(vector3.X, vector3.Y, vector3.Z);
        Gl.Uniform3(location, sysVector3);
    }
    
}
