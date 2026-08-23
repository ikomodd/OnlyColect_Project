shader_type canvas_item;

const vec4 progressColor = vec4(0.0f, 1.0f, 0.0f, 1.0f);
const vec4 backgroundColor = vec4(0.15f, 0.15f, 0.15f, 1.0f);

uniform float u_progress = 0.7;

void vertex() {
	// Called for every vertex the material is visible on.
}

void fragment() {
	
	if (UV.x <= u_progress)
		COLOR = progressColor;
	else
		COLOR = backgroundColor;
}

//void light() {
//	// Called for every pixel for every light affecting the CanvasItem.
//	// Uncomment to replace the default light processing function with this one.
//}
