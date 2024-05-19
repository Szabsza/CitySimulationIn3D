#version 330 core
layout (location = 0) in vec3 aPos;
layout (location = 1) in vec3 aNormal;
layout (location = 2) in vec2 aTexCoord;

uniform mat4 uProjection;
uniform mat4 uModel;
uniform mat3 uNormal;
uniform mat4 uView;

out vec2 outTex;
out vec3 outNormal;
out vec3 outWorldPosition;

void main()
{
    gl_Position = uProjection * uView * uModel * vec4(aPos, 1.0);
    outTex = aTexCoord;
    outNormal = mat3(transpose(inverse(uModel))) * aNormal;
    outWorldPosition = vec3(uModel*vec4(aPos, 1.0));
}