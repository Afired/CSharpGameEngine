#type vertex
#version 330 core
layout (location = 0) in vec3 aPosition;
layout (location = 1) in vec2 aTexCoord;
out vec2 vTexCoord;

uniform mat4 projection;
uniform mat4 model;

void main() 
{
    vTexCoord = aTexCoord;
    gl_Position = projection * model * vec4(aPosition.xyz, 1.0);
}

#type fragment
#version 330 core
out vec4 FragColor;
in vec2 vTexCoord;

uniform sampler2D u_Texture;

void main() 
{
    // FragColor = vec4(vTexCoord, 1.0, 1.0);
    FragColor = texture(u_Texture, vTexCoord);
}
