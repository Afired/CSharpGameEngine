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
const float sampleRate = 0.005;
const float radius = 0.05;

void main()
{
    //todo: replace with better blur -> downsample, upsample
    
    float iterations = (radius / sampleRate) * (radius / sampleRate) * 2.0;
    vec4 sum = vec4(0.0);
    for (float x = -radius; x <= radius; x += sampleRate) {
        for (float y = -radius; y <= radius; y += sampleRate) {
            vec4 neighboringPixel = texture(screenTexture, TexCoords + vec2(x, y));
            //if(length(neighboringPixel) > threshold)
                sum += neighboringPixel / iterations;
        }
    }
    //if(length(texture(screenTexture, TexCoords)) > 3.0)
    //    sum += normalize(texture(screenTexture, TexCoords)) * 2.0;
    //else
    //    sum += texture(screenTexture, TexCoords);
    FragColor = sum;
    //todo: tonemapping
}
