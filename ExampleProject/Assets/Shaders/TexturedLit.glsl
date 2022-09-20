#type vertex
#version 330 core
layout (location = 0) in vec3 aPosition;
layout (location = 1) in vec2 aTexCoord;
layout (location = 2) in vec3 aNormal;
out vec4 vertexColor;
out vec2 vTexCoord;
out vec3 vNormal;

uniform mat4 projection;
uniform mat4 model;

void main() 
{
    vertexColor = vec4(1.0);
    vTexCoord = aTexCoord;
    vNormal = aNormal;
    
    gl_Position = projection * model * vec4(aPosition.xyz, 1.0);
}


#type fragment
#version 330 core
out vec4 FragColor;
in vec4 vertexColor;
in vec2 vTexCoord;
in vec3 vNormal;

uniform sampler2D u_Texture;

void main() 
{
    // normalize vNormal?
    float ambientLightStrength = 0.1;
    vec3 lightDir = vec3(-1.0, -1.0, 0.0);

    vec3 ambient = ambientLightStrength * vec3(texture(u_Texture, vTexCoord));

    vec3 diffuse = max(dot(vNormal, lightDir), 0.0) * vec3(texture(u_Texture, vTexCoord));
    
    vec3 result = diffuse + ambient;
    FragColor = vec4(result, 1.0);
}
