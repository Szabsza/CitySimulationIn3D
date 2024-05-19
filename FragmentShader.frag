#version 330 core
uniform sampler2D uTexture;
uniform float ambientIntensity;
uniform vec3 viewPos;
uniform vec3 lightPosition1;
uniform vec3 lightDirection1;
uniform vec3 lightPosition2;
uniform vec3 lightDirection2;

uniform bool areHeadlightsTurnedOn;
uniform bool isNightMode;

in vec2 outTex;
in vec3 outNormal;
in vec3 outWorldPosition;

out vec4 FragColor;

struct Light {
    vec3 position;
    vec3 direction;
    float cutOff;
};

uniform Light streetlights[100];
uniform Light headlights[2];

uniform Light policeLights[5];
uniform bool isChasing[5];
uniform vec3 policeLightColor;

uniform bool isParticle;
uniform vec3 particleColor;


vec3 calculateLighting(vec3 lightColor, vec3 lightPosition, vec3 lightDirection, float lightCutOff, vec3 viewDir, vec3 norm) 
{
    float constant = 1.0;
    float linear = 0.09;
    float quadratic = 0.032;
    float shininess = 300;

    vec3 resultColor = vec3(0, 0, 0);

    vec3 lightDir = normalize(lightPosition - outWorldPosition);
    float theta = dot(lightDir, normalize(-lightDirection));

    if (theta > lightCutOff)
    {
        float diffuseStrength = 0.8;
        vec3 lightDir = normalize(lightPosition - outWorldPosition);
        float diff = max(dot(norm, lightDir), 0.0);
        vec3 diffuse = diff * lightColor * diffuseStrength;

        float specularStrength = 0.2;
        vec3 reflectDir = reflect(-lightDir, norm);
        float spec = pow(max(dot(viewDir, reflectDir), 0.0), shininess);
        vec3 specular = specularStrength * spec * lightColor;

        float distance = length(lightPosition - outWorldPosition);
        float attenuation = 1.0 / (constant + linear * distance + quadratic * (distance * distance));

        diffuse *= attenuation;
        specular *= attenuation;

        resultColor += diffuse + specular;
    }
    
    return resultColor;
}

void main()
{
    if (isParticle) {
        FragColor = vec4(particleColor, 1.0);
        return;
    }
    
    
    vec3 lightColor = vec3(1, 1, 1);
    vec3 streetLightColor = vec3(0.98, 0.93, 0.54);
    
    vec3 ambient = ambientIntensity  * lightColor;
    vec3 result = ambient;

    vec3 viewDir = normalize(viewPos - outWorldPosition);
    vec3 norm = normalize(outNormal);
    
    if (areHeadlightsTurnedOn) {
        result += calculateLighting(lightColor, lightPosition1, lightDirection1, 0.86, viewDir, norm);
        result += calculateLighting(lightColor, lightPosition2, lightDirection2, 0.86, viewDir, norm);
    }

    if (isNightMode) {
        for (int i = 0; i < 100; i++) {
            result += calculateLighting(streetLightColor, streetlights[i].position, streetlights[i].direction, streetlights[i].cutOff, viewDir, norm);
        }
    }

    for (int i = 0; i < 5; i++) {
        if (isChasing[i]) {
            result += calculateLighting(policeLightColor, policeLights[i].position, policeLights[i].direction, policeLights[i].cutOff, viewDir, norm);
        }
    }
    
    vec4 textColor = texture(uTexture, outTex);
    FragColor = vec4(result, 1.0) * textColor;
}
