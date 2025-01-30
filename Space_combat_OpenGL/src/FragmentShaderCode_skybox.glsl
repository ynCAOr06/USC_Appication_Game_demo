#version 430

out vec4 outColor;

in vec3 vertexPositionWorld;

uniform samplerCube myTextureSampler0;

void main()
{
	//texture
	outColor = texture(myTextureSampler0, vertexPositionWorld);
}
