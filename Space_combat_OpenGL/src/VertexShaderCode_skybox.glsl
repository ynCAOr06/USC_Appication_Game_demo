#version 430

in layout(location=0) vec3 position;

out vec3 vertexPositionWorld;

uniform mat4 modelTransformMatrix;
uniform mat4 viewMatrix;
uniform mat4 projectionMatrix;

void main()
{
	vec4 v = vec4(position, 1.0);
	vec4 outPosition = projectionMatrix * viewMatrix * modelTransformMatrix * v;
	gl_Position = outPosition;
	vertexPositionWorld = position;
} 