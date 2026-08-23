shader_type canvas_item;

const vec3 baseColor = vec3(1.0f, 0.6f, 0.0f);
const vec2 center = vec2(0.5f, 0.5f);

uniform float u_size;

void vertex() {
	// Called for every vertex the material is visible on.
}

void fragment() {
	
	float pulse = 4.0f - u_size - (sin(TIME * 10.0f * u_size) + 1.0f) * 0.5f;
	
	float dis = length(center - UV) * pulse;
	
	COLOR = vec4(baseColor.r, baseColor.g, baseColor.b, 1.0f - dis);
}

//void light() {
//	// Called for every pixel for every light affecting the CanvasItem.
//	// Uncomment to replace the default light processing function with this one.
//}
