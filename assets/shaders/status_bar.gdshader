shader_type canvas_item;

uniform vec4 u_frontBarColor = vec4(1.f, 0.f, 0.f, 1.f);
uniform vec4 u_backgroundColor = vec4(0.f, 0.f, 0.f, 1.f);

uniform int u_damageType = 1;

uniform float u_frontBarStatus = 0.75;
uniform float u_backBarStatus = 0.5;

void vertex() {
	// Called for every vertex the material is visible on.
}

void fragment() {
	
	vec4 backBarColor;
	
	// 0: UNDEFINED_DAMAGE; 1: FIRE_DAMAGE; 2: HIT_DAMAGE
	switch (u_damageType) {
		case 0: backBarColor = vec4(0.1f, 0.1f, 0.1f, 1.0f);
		break;
		case 1: backBarColor = vec4(0.9f, 0.5f, 0.0f, 1.0f);
		break;
		case 2: backBarColor = vec4(0.6f, 0.0f, 0.0f, 1.0f);
		break;
	}
	
	float yDistance = abs(0.5f - UV.y);
	vec4 yColor = vec4(0.0f, 0.0f, 0.0f, yDistance);
	
	if (UV.x > u_backBarStatus && UV.x < u_frontBarStatus)
		COLOR = vec4(0.0f, 1.0f, 0.0f, 1.0f) - yColor;
	else if (UV.x < u_frontBarStatus)
		COLOR = u_frontBarColor - yColor;
	else if (UV.x > u_frontBarStatus && UV.x < u_backBarStatus)
		COLOR = backBarColor - yColor;
	else
		COLOR = u_backgroundColor - yColor;
}

//void light() {
//	// Called for every pixel for every light affecting the CanvasItem.
//	// Uncomment to replace the default light processing function with this one.
//}
