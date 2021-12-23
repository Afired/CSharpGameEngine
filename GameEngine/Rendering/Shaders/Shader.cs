using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using GameEngine.Numerics;
using Silk.NET.OpenGL;
using GL = OpenGL.GL;

namespace GameEngine.Rendering.Shaders;

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

public class Shader {

    private uint _programID;


    public Shader(string vertexCode, string fragmentCode) {
        (GLEnum shaderType, string shaderSrc)[] shaderInfo = { (GLEnum.VertexShader, vertexCode), (GLEnum.FragmentShader, fragmentCode) };
        Compile(shaderInfo);
    }
    
    public Shader(string filePath) {
        string contents = ReadFileWithFileStream(filePath);
        (GLEnum, string)[] shaderInfo = SplitIntoShader(contents);
        Compile(shaderInfo);
    }

    private void Compile((GLEnum shaderType, string shaderSrc)[] shaderInfo) {
        uint[] shaderIDs = new uint[shaderInfo.Length];
        
        for(int i = 0; i < shaderInfo.Length; i++) {

            int type = (int) shaderInfo[i].shaderType;
            string src = shaderInfo[i].shaderSrc;

            shaderIDs[i] = GL.glCreateShader(type);
            GL.glShaderSource(shaderIDs[i], src);
            GL.glCompileShader(shaderIDs[i]);

            int[] status = GL.glGetShaderiv(shaderIDs[i], GL.GL_COMPILE_STATUS, 1);
            if(status[0] == 0) {
                string error = GL.glGetShaderInfoLog(shaderIDs[i]);
                throw new Exception($"Shader failed to compile {error}");
            }
            
        }
        
        _programID = GL.glCreateProgram();
        for(int i = 0; i < shaderIDs.Length; i++) {
            GL.glAttachShader(_programID, shaderIDs[i]);
        }
        GL.glLinkProgram(_programID);
        
        // Delete Shaders
        for(int i = 0; i < shaderIDs.Length; i++) {
            GL.glDetachShader(_programID, shaderIDs[i]);
            GL.glDeleteShader(shaderIDs[i]);
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

    private static (GLEnum shaderType, string shaderSrc)[] SplitIntoShader(string contents) {
        const string SHADER_TYPE_KEYWORD = "#type";
        string[] shaderSrcs = contents.Split(SHADER_TYPE_KEYWORD, StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
        (GLEnum shaderType, string shaderSrc)[] result = new (GLEnum shaderType, string shaderSrc)[shaderSrcs.Length];
        for(int i = 0; i < shaderSrcs.Length; i++) {
            string keyword = shaderSrcs[i].GetFirstWord();
            Debug.Assert(!string.IsNullOrEmpty(keyword));

            result[i].shaderType = ShaderTypeFromString(keyword);
            result[i].shaderSrc = shaderSrcs[i].Replace(keyword, "");
        }
        return result;
    }

    private static GLEnum ShaderTypeFromString(string type) => type switch {
        "vertex" => GLEnum.VertexShader,
        "fragment" => GLEnum.FragmentShader,
        "pixel" => GLEnum.FragmentShader,
        _ => throw new Exception($"unsupported shader type: '{type}'")
    };
    
    public void Use() {
        GL.glUseProgram(_programID);
    }

    public void SetMatrix4x4(string uniformName, Matrix4x4 mat) {
        int location = GL.glGetUniformLocation(_programID, uniformName);
        GL.glUniformMatrix4fv(location, 1, false, mat.ToArray());
    }

    public void SetInt(string uniformName, int value) {
        int location = GL.glGetUniformLocation(_programID, uniformName);
        GL.glUniform1i(location, value);
    }

    public void SetFloat(string uniformName, float value) {
        int location = GL.glGetUniformLocation(_programID, "time");
        GL.glUniform1f(location, value);
    }

}

public interface IShader {
    
    public Shader Shader { get; set; }
    
}
