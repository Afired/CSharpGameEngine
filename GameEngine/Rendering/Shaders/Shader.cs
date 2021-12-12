using GameEngine.Debugging;
using GameEngine.Numerics;
using OpenGL;

namespace GameEngine.Rendering.Shaders; 

public class Shader {
    
    public uint ProgramID { get; set; }
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

        ProgramID = GL.glCreateProgram();
        GL.glAttachShader(ProgramID, vs);
        GL.glAttachShader(ProgramID, fs);
        
        GL.glLinkProgram(ProgramID);
        
        // Delete Shaders
        
        GL.glDetachShader(ProgramID, vs);
        GL.glDetachShader(ProgramID, fs);
        GL.glDeleteShader(vs);
        GL.glDeleteShader(fs);
    }

    public void Use() {
        GL.glUseProgram(ProgramID);
    }

    public void SetMatrix4x4(string uniformName, Matrix4x4 mat) {
        int location = GL.glGetUniformLocation(ProgramID, uniformName);
        GL.glUniformMatrix4fv(location, 1, false, GetMatrix4x4Values(mat));
    }
    
    private float[] GetMatrix4x4Values(Matrix4x4 mat) {
        return new float[] {
            mat.M11, mat.M12, mat.M13, mat.M14,
            mat.M21, mat.M22, mat.M23, mat.M24,
            mat.M31, mat.M32, mat.M33, mat.M34,
            mat.M41, mat.M42, mat.M43, mat.M44
        };
    }
    
}
