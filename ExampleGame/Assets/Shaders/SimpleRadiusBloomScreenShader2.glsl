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

const float threshold = 2.0;
const float samples = 10.0;
const float radius = 0.1;
const float bloomStrength = 1.0;

void main()
{
    //todo: replace with better blur -> downsample, upsample
    const float PI = 3.1415926;
    vec4 sum = vec4(0.0);
    int count = 0;
    float sampleRate = radius / samples;
    for(float r = 0.0; r <= radius; r += sampleRate) {
        for(float u = 0.0; u <= 2.0 * PI; u += sampleRate * 2 * PI) {
            float x = sin(u) * r;
            float y = cos(u) * r;
            vec4 color = texture(screenTexture, TexCoords + vec2(x, y));
            if(length(color.rgb) > threshold) {
                sum += color;
            }
            count++;
        }
    }
    sum = (sum / count) * bloomStrength;
    //if(length(sum) > 3.0)
    //    sum = normalize(sum) * 3.0;
    //else
    //    sum += sum;
    
    if(length(texture(screenTexture, TexCoords)) > 3.0)
        sum += normalize(texture(screenTexture, TexCoords)) * 1.0;
    else
        sum += texture(screenTexture, TexCoords);
    FragColor = sum;
    //todo: tonemapping
}
