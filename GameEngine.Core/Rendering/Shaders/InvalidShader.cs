namespace GameEngine.Core.Rendering.Shaders; 

internal static class InvalidShader {
    
    internal const string VERTEX_SHADER = @"

#version 330 core
layout (location = 0) in vec3 aPosition;
layout (location = 1) in vec2 aTexCoord;
out vec4 vertexColor;
out vec2 vTexCoord;

uniform mat4 projection;
uniform mat4 view;
uniform mat4 model;

void main() 
{
    vertexColor = vec4(1.0);
    vTexCoord = aTexCoord;
    gl_Position = projection * view * model * vec4(aPosition.xyz, 1.0);
}

";

    internal const string FRAGMENT_SHADER = @"

#version 330 core
out vec4 FragColor;
in vec4 vertexColor;
in vec2 vTexCoord;
uniform sampler2D u_Texture;

void main() 
{
    vec4 magenta = vec4(1.0, 0.0, 1.0, 1.0);
    FragColor = magenta;
}

";

}
