shader_type canvas_item;
render_mode blend_mix, unshaded;

uniform sampler2D front_texture : source_color, filter_linear, repeat_disable;
uniform sampler2D back_texture : source_color, filter_linear, repeat_disable;

uniform float progress : hint_range(0.0, 1.0, 0.001) = 0.0;
uniform bool reverse = false;
uniform float theta_min : hint_range(0.08, 1.5708, 0.001) = 0.35;
uniform float A_near : hint_range(-1.0, 0.0, 0.001) = -0.15;
uniform float A_far : hint_range(-6.0, 0.0, 0.001) = -2.0;
uniform float light_strength : hint_range(0.0, 1.0, 0.01) = 0.35;
uniform vec2 page_size = vec2(512.0, 512.0);

varying vec3 v_normal;

vec3 cone_deform(vec2 vi, float theta, float A, float rho) {
	float R = length(vec2(vi.x, vi.y - A));
	if (R < 0.0001) {
		return vec3(vi.x, vi.y, 0.0);
	}
	float sin_theta = max(sin(theta), 0.0001);
	float r = R * sin_theta;
	float beta = asin(clamp(vi.x / R, 0.0, 1.0)) / sin_theta;
	vec3 v1;
	if (beta > PI) {
		v1.x = 0.0;
		v1.y = R + A - r * (1.0 - cos(PI)) * sin_theta;
		v1.z = r * (1.0 - cos(PI)) * cos(theta);
	} else {
		v1.x = r * sin(beta);
		v1.y = R + A - r * (1.0 - cos(beta)) * sin_theta;
		v1.z = r * (1.0 - cos(beta)) * cos(theta);
	}
	float c = cos(rho);
	float s = sin(rho);
	return vec3(v1.x * c - v1.z * s, v1.y, v1.x * s + v1.z * c);
}

void vertex() {
	vec2 page = max(page_size, vec2(1.0));
	float t = clamp(progress, 0.0, 1.0);
	float rho = t * PI;
	float wave = sin(t * PI);
	float theta = mix(PI * 0.5, max(theta_min, 0.08), wave);
	float A = mix(A_near, A_far, wave) * page.y;

	vec2 vi = vec2(VERTEX.x, page.y - VERTEX.y);
	vec3 vo = cone_deform(vi, theta, A, rho);
	vec3 vo_x = cone_deform(vi + vec2(1.0, 0.0), theta, A, rho);
	vec3 vo_y = cone_deform(vi + vec2(0.0, 1.0), theta, A, rho);
	v_normal = cross(vo_x - vo, vo_y - vo);

	if (reverse) {
		vo.x = -vo.x;
	}

	VERTEX = vec2(vo.x, page.y - vo.y);
}

void fragment() {
	vec3 n = v_normal;
	float nlen = length(n);
	float nz = nlen > 0.0001 ? n.z / nlen : 1.0;
	bool show_back = nz < 0.0;

	vec4 col = show_back
		? texture(back_texture, vec2(1.0 - UV.x, UV.y))
		: texture(front_texture, UV);

	float shade = mix(1.0, 0.55 + 0.45 * abs(nz), light_strength);
	COLOR = vec4(col.rgb * shade, col.a);
}
