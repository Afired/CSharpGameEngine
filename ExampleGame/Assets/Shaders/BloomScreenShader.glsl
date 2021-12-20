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

const float blurSizeH = 1.0 / 300.0;
const float blurSizeV = 1.0 / 200.0;
void main()
{
    //todo: replace with better blur -> downsample, upsample
    vec4 sum = vec4(0.0);
    for (int x = -4; x <= 4; x++)
        for (int y = -4; y <= 4; y++)
            sum += texture(screenTexture, vec2(TexCoords.x + x * blurSizeH, TexCoords.y + y * blurSizeV)) / 81.0;
    FragColor = sum;
    
    //todo: tonemapping
}
