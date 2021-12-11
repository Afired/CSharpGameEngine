using System;
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
            Console.WriteLine($"FAILED TO COMPILE VERTEX SHADER: {error}");
        }
        
        uint fs = GL.glCreateShader(GL.GL_FRAGMENT_SHADER);
        GL.glShaderSource(fs, _fragmentCode);
        GL.glCompileShader(fs);
        
        status = GL.glGetShaderiv(fs, GL.GL_COMPILE_STATUS, 1);
        if(status[0] == 0) {
            string error = GL.glGetShaderInfoLog(fs);
            Console.WriteLine($"FAILED TO COMPILE FRAGMENT SHADER: {error}");
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
    
}
