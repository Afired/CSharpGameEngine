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

const float threshold = 3.0;

const float blurSizeH = 1.0 / 500.0;
const float blurSizeV = 1.0 / 500.0;
void main()
{
    //todo: replace with better blur -> downsample, upsample
    vec4 sum = vec4(0.0);
    sum += texture(screenTexture, TexCoords);
    for (int x = -8; x <= 8; x++) {
        for (int y = -8; y <= 8; y++) {
            vec4 neighboringPixel = texture(screenTexture, vec2(TexCoords.x + x * blurSizeH, TexCoords.y + y * blurSizeV));
            if(length(neighboringPixel.rgb) > threshold)
                sum += neighboringPixel / 400.0;
        }
    }
    FragColor = sum;
    //todo: tonemapping
}
