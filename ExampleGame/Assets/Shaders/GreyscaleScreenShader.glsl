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

const float greyscaleFactor = 1.0;

void main()																
{
	vec4 sample =  texture(screenTexture, TexCoords);
	float grey = 0.21 * sample.r + 0.71 * sample.g + 0.07 * sample.b;
	FragColor = vec4(sample.rgb * (1.0 - greyscaleFactor) + (grey * greyscaleFactor), 1.0);
}	