#type vertex
#version 330 core
layout (location = 0) in vec2 aPos;
layout (location = 1) in vec2 aTexCoords;

out vec2 TexCoords;

void main()
{
    gl_Position = vec4(aPos.x, aPos.y, 0.0, 1.0); 
    TexCoords = aTexCoords;
}

#type fragment
#version 330 core
out vec4 FragColor;
in vec2 TexCoords;
uniform sampler2D screenTexture;
uniform float time;

void main()
{
    // credit: https://www.shadertoy.com/view/lsKSWR
    vec2 uv = TexCoords.xy / vec2(1.0, 1.0);
    uv *=  1.0 - uv.yx;   //vec2(1.0)- uv.yx; -> 1.-u.yx; Thanks FabriceNeyret !
    float vig = uv.x*uv.y * 15.0; // multiply with sth for intensity
    vig = pow(vig, 0.25); // change pow for modifying the extend of the  vignette
    FragColor = vec4(vig); 
}
