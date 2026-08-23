shader_type canvas_item;

uniform bool u_enabled;

void vertex() {
	// Called for every vertex the material is visible on.
}

void fragment() {
	
	if (u_enabled) {
		
		float motion = fract(TIME) / (16.0f);
		vec2 newUV = UV + vec2(motion, 0.0f);
		
		COLOR = texture(TEXTURE, newUV);
	}
}

//void light() {
//	// Called for every pixel for every light affecting the CanvasItem.
//	// Uncomment to replace the default light processing function with this one.
//}
