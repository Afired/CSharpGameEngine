using GameEngine.Debugging;
using GameEngine.Numerics;
using OpenGL;

namespace GameEngine.Rendering.Shaders; 

public class Shader {

    private uint _programID;
    private string _vertexCode;
    private string _fragmentCode;


    public Shader(string vertexCode, string fragmentCode) {
        _vertexCode = vertexCode;
        _fragmentCode = fragmentCode;
    }

    public void Load() {
        uint vs = GL.glCreateShader(GL.GL_VERTEX_SHADER);
        GL.glShaderSource(vs, _vertexCode);
        GL.glCompileShader(vs);

        int[] status = GL.glGetShaderiv(vs, GL.GL_COMPILE_STATUS, 1);
        if(status[0] == 0) {
            string error = GL.glGetShaderInfoLog(vs);
            throw new ShaderFailedToCompileException(error);
        }
        
        uint fs = GL.glCreateShader(GL.GL_FRAGMENT_SHADER);
        GL.glShaderSource(fs, _fragmentCode);
        GL.glCompileShader(fs);
        
        status = GL.glGetShaderiv(fs, GL.GL_COMPILE_STATUS, 1);
        if(status[0] == 0) {
            string error = GL.glGetShaderInfoLog(fs);
            throw new ShaderFailedToCompileException(error);
        }

        _programID = GL.glCreateProgram();
        GL.glAttachShader(_programID, vs);
        GL.glAttachShader(_programID, fs);
        
        GL.glLinkProgram(_programID);
        
        // Delete Shaders
        
        GL.glDetachShader(_programID, vs);
        GL.glDetachShader(_programID, fs);
        GL.glDeleteShader(vs);
        GL.glDeleteShader(fs);
    }

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

}

public interface IShader {
    
    public Shader Shader { get; set; }
    
}
