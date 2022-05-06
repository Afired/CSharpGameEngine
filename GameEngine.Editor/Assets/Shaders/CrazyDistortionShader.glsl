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

// GeeXLab built-in uniform, width of
// the current render target
float rt_w = 1000.0; 
// GeeXLab built-in uniform, height of
// the current render target
 float rt_h = 1000.0; 

// Swirl effect parameters
 float radius = 400.0;
 float angle = 0.8;
 vec2 center = vec2(500.0, 500.0);

//credit: https://www.geeks3d.com/20110428/shader-library-swirl-post-processing-filter-in-glsl/
vec4 PostFX(sampler2D tex, vec2 uv, float time)
{
  vec2 texSize = vec2(rt_w, rt_h);
  vec2 tc = uv * texSize;
  tc -= center;
  float dist = length(tc);
  if (dist < radius) 
  {
    float percent = (radius - dist) / radius;
    float theta = percent * percent * angle * 8.0;
    float s = sin(theta);
    float c = cos(theta);
    tc = vec2(dot(tc, vec2(c, -s)), dot(tc, vec2(s, c)));
  }
  tc += center;
  vec3 color = texture2D(screenTexture, tc / texSize).rgb;
  return vec4(color, 1.0);
}

void main (void)
{
  vec2 uv = TexCoords;
  FragColor = PostFX(screenTexture, uv, time);
}