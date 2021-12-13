﻿namespace GameEngine.Rendering.Shaders; 

internal static class DefaultShader {
    
    private const string VERTEX_SHADER = @"

#version 330 core
layout (location = 0) in vec2 aPosition;
layout (location = 1) in vec3 aColor;
out vec4 vertexColor;

uniform mat4 projection;
uniform mat4 model;

void main() 
{
    vertexColor = vec4(aColor.rgb, 1.0);
    gl_Position = projection * model * vec4(aPosition.xy, 0, 1.0);
}

";

    private const string FRAGMENT_SHADER = @"

#version 330 core
out vec4 FragColor;
in vec4 vertexColor;

void main() 
{
    FragColor = vertexColor;
}

";

    internal static void Initialize() {
        Shader shader = new Shader(VERTEX_SHADER, FRAGMENT_SHADER);
        ShaderRegister.Register("default", shader);
        shader.Load();
    }
    
}
