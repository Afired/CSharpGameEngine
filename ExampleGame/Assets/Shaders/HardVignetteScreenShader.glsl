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

vec4 applyVignette(vec4 color)
{
    vec2 position = (gl_FragCoord.xy / 800.0) - vec2(0.5);           
    float dist = length(position);

    float radius = 0.5;
    float softness = 0.02;
    float vignette = smoothstep(radius, radius - softness, dist);

    color.rgb = color.rgb - (1.0 - vignette);

    return color;
}

void main()
{
    // hard, credit: https://stackoverflow.com/questions/52762754/how-to-render-a-circular-vignette-with-glsl
    vec4 color = texture(screenTexture, TexCoords);
    color = applyVignette(color);
    FragColor = color;
}
