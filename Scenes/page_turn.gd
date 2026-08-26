@tool
extends Control

@export_range(0.0, 1.0, 0.001) var progress: float = 0.0:
	set(value):
		progress = clampf(value, 0.0, 1.0)
		_apply_params()

@export var reverse: bool = false:
	set(value):
		reverse = value
		_apply_params()

@export_range(8, 64, 1) var grid_subdiv: int = 40:
	set(value):
		grid_subdiv = value
		_page_size = Vector2.ZERO
		_sync_layout()

@onready var _front_viewport: SubViewport = $front_face
@onready var _back_viewport: SubViewport = $back_face
@onready var _turning_page: MeshInstance2D = $turning_page

var _page_size := Vector2.ZERO
var _textures_bound := false


func _ready() -> void:
	_bind_textures()
	_sync_layout()


func _notification(what: int) -> void:
	if what == NOTIFICATION_RESIZED:
		_sync_layout()


func _sync_layout() -> void:
	if not is_node_ready():
		return
	var total := size
	if total.x < 4.0 or total.y < 2.0:
		total = custom_minimum_size
	var page := Vector2(total.x * 0.5, total.y)
	if page.x < 2.0 or page.y < 2.0:
		return

	var page_i := Vector2i(maxi(int(round(page.x)), 2), maxi(int(round(page.y)), 2))
	_front_viewport.size = page_i
	_back_viewport.size = page_i
	_turning_page.position = Vector2(page.x, 0.0)

	if not page.is_equal_approx(_page_size):
		_page_size = page
		_turning_page.mesh = _build_page_mesh(page)

	_apply_params()


func _apply_params() -> void:
	if not is_node_ready():
		return
	var mat := _turning_page.material as ShaderMaterial
	if mat == null:
		return
	mat.set_shader_parameter("progress", progress)
	mat.set_shader_parameter("reverse", reverse)
	if _page_size.x > 0.0:
		mat.set_shader_parameter("page_size", _page_size)


func _bind_textures() -> void:
	if _textures_bound:
		return
	var mat := _turning_page.material as ShaderMaterial
	if mat == null:
		return
	mat.set_shader_parameter("front_texture", _front_viewport.get_texture())
	mat.set_shader_parameter("back_texture", _back_viewport.get_texture())
	_textures_bound = true


func _build_page_mesh(page: Vector2) -> ArrayMesh:
	var res_x := maxi(grid_subdiv, 8)
	var res_y := maxi(grid_subdiv, 8)
	var verts := PackedVector3Array()
	var uvs := PackedVector2Array()
	var indices := PackedInt32Array()
	verts.resize((res_x + 1) * (res_y + 1))
	uvs.resize(verts.size())

	for j in range(res_y + 1):
		for i in range(res_x + 1):
			var idx := j * (res_x + 1) + i
			var u := float(i) / float(res_x)
			var v := float(j) / float(res_y)
			verts[idx] = Vector3(u * page.x, v * page.y, 0.0)
			uvs[idx] = Vector2(u, v)

	indices.resize(res_x * res_y * 6)
	var t := 0
	for j in range(res_y):
		for i in range(res_x):
			var i0 := j * (res_x + 1) + i
			var i1 := i0 + 1
			var i2 := i0 + res_x + 1
			var i3 := i2 + 1
			indices[t] = i0
			indices[t + 1] = i1
			indices[t + 2] = i2
			indices[t + 3] = i1
			indices[t + 4] = i3
			indices[t + 5] = i2
			t += 6

	# Unindexed corners expand the 2D cull rect across the full spread.
	# ArrayMesh.custom_aabb is ignored by MeshInstance2D.get_aabb().
	verts.append(Vector3(-page.x, -page.y * 0.15, 0.0))
	verts.append(Vector3(-page.x, page.y * 1.15, 0.0))
	verts.append(Vector3(page.x, -page.y * 0.15, 0.0))
	verts.append(Vector3(page.x, page.y * 1.15, 0.0))
	uvs.append(Vector2(0.0, 0.0))
	uvs.append(Vector2(0.0, 1.0))
	uvs.append(Vector2(1.0, 0.0))
	uvs.append(Vector2(1.0, 1.0))

	var arrays: Array = []
	arrays.resize(Mesh.ARRAY_MAX)
	arrays[Mesh.ARRAY_VERTEX] = verts
	arrays[Mesh.ARRAY_TEX_UV] = uvs
	arrays[Mesh.ARRAY_INDEX] = indices

	var mesh := ArrayMesh.new()
	mesh.add_surface_from_arrays(Mesh.PRIMITIVE_TRIANGLES, arrays)
	return mesh
