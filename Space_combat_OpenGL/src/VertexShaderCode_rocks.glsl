#version 430

in layout(location=0) vec3 position;
in layout(location=1) vec2 uv;
in layout(location=2) vec3 normal;
in layout(location=3) mat4 instanceMatrix;

uniform mat4 modelTransformMatrix;
uniform mat4 viewMatrix;
uniform mat4 projectionMatrix;

out vec2 UV;
out vec3 normalWorld;
out vec3 vertexPositionWorld;

void main()
{
	vec4 v = vec4(position, 1.0);
	vec4 newPosition = instanceMatrix * v;
	vec4 outPosition = projectionMatrix * viewMatrix * modelTransformMatrix * newPosition;
	gl_Position = outPosition;	
	UV = uv;

	vec4 normal_temp = modelTransformMatrix * vec4 (normal, 0);
	normalWorld = normal_temp.xyz;
	vertexPositionWorld = newPosition.xyz;
}
