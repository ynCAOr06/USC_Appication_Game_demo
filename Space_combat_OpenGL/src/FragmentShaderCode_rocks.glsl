#version 430

out vec4 outColor;
in vec2 UV;
in vec3 normalWorld;
in vec3 vertexPositionWorld;

uniform sampler2D myTextureSampler0;
uniform vec3 lightIntensity;
uniform vec3 ambientLight;
uniform vec3 lightPositionWorld;
uniform vec3 eyePositionWorld;

struct DirLight {
	vec3 direction;

	vec3 ambient;
	vec3 diffuse;
	vec3 specular;
};

struct PointLight {
	vec3 position;
	vec3 attenuation;

	vec3 ambient;
	vec3 diffuse;
	vec3 specular;
};

uniform DirLight dirLight;
uniform PointLight pointLight;

vec3 CalcDirLight(DirLight dirLight)
{
	vec3 lightDir = normalize(-dirLight.direction);

	float brightness = dot(lightDir, normalize(normalWorld));
	vec3 diffuseLight = clamp(vec3(brightness, brightness, brightness), 0, 1);

	float specularStrength = 0.1f;
	vec3 lightColor = vec3(0.953f, 0.910f, 0.605f);
	vec3 reflectedLightDir = reflect(-lightDir, normalWorld);
	vec3 viewDir = normalize(eyePositionWorld - vertexPositionWorld);
	float s = clamp(dot(reflectedLightDir, viewDir), 0, 1);
	s = pow(s, 32);
	vec3 specularLight = specularStrength * s * lightColor;

	vec3 ambient = dirLight.ambient;
	vec3 diffuse = dirLight.diffuse * diffuseLight;
	vec3 specular = dirLight.specular * specularLight;
	
	return (ambient + diffuse + specular);
}

vec3 CalcPointLight(PointLight pointLight)
{
	vec3 lightDir = normalize(pointLight.position - vertexPositionWorld);
	float distance = length(lightDir);
	vec3 distanceVec = vec3(1.0f, distance, distance * distance);
	float attenuation = 1.0f / dot(pointLight.attenuation, distanceVec);

	float brightness = dot(lightDir, normalize(normalWorld));
	vec3 diffuseLight = clamp(vec3(brightness, brightness, brightness), 0, 1);

	float specularStrength = 0.8f;
	vec3 lightColor = vec3(0.332f, 0.805f, 0.996f);
	vec3 reflectedLightDir = reflect(-lightDir, normalWorld);
	vec3 viewDir = normalize(eyePositionWorld - vertexPositionWorld);
	float s = clamp(dot(reflectedLightDir, viewDir), 0, 1);
	s = pow(s, 64);
	vec3 specularLight = specularStrength * s * lightColor;

	vec3 ambient = attenuation * pointLight.ambient;
	vec3 diffuse = attenuation * pointLight.diffuse * diffuseLight;
	vec3 specular = attenuation * pointLight.specular * specularLight;
	
	return (ambient + diffuse + specular);
}



void main()
{
	//texture
	vec3 texture = texture(myTextureSampler0, UV).rgb;

	vec3 finalColor = (lightIntensity * CalcDirLight(dirLight) + 2.0 * CalcPointLight(pointLight)) * texture;
	outColor = vec4 (finalColor, 1.0);
}
