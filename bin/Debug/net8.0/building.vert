#version 330 core
layout (location = 0) in vec3 aPos;
layout (location = 1) in vec2 aTexCoord;
out vec4 ourColor;
out vec2 TexCoord;
uniform mat4 proj;
uniform mat4 model;
uniform mat4 view;
void main()
{
    gl_Position = proj * view * model * vec4(aPos, 1.0);
    TexCoord = aTexCoord;
}