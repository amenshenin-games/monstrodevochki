# Временная заплатка до реализации нормальной визуальной системы диалога, as a prove of concept
extends Node

@onready var character_image = $"../Images/HBoxContainer/CharacterImage"
@onready var background = $"../Background"


# Called when the node enters the scene tree for the first time.
func _ready() -> void:
	SignalBus.connect("add_image", add_image)
	SignalBus.connect("change_image", change_image)
	SignalBus.connect("change_background", change_background)



func add_image(image_name: String) -> void:
	var new_image = character_image.duplicate()
	new_image.add_to_group('character_image')
	#get_parent().add_child(new_image)
	get_node("../Images/HBoxContainer").add_child(new_image)
	#get_parent().get_node("../Images").add_child(new_image)
	_change_image(new_image, image_name)

func change_image(image_name: String) -> void:
	var images = get_tree().get_nodes_in_group('character_image')
	for image in images:
		image.queue_free()
	_change_image(character_image, image_name)

func change_background(image_name: String) -> void:
	_change_image(background, image_name)


func _change_image(image_node: TextureRect, image_name: String) -> void:
	print("[VisualSystem] changing ", image_node, " to ", image_name)
	if image_node == null:
		print("[VisualSystem] node ", image_node, " not found")
		return
	
	if image_name == "0" or image_name == "none":
		image_node.visible = false
	else:
		image_node.visible = true
		var image_texture: Resource
		if image_node == background:
			print("background")
			if ResourceLoader.exists("res://Assets/Art/Backgrounds/" + image_name + ".png"):
				image_texture = load("res://Assets/Art/Backgrounds/" + image_name + ".png")
			else:
				image_texture = load("res://Assets/Art/Backgrounds/" + image_name + ".jpg")
				
		else:
			print("character")
			image_texture = load("res://Assets/Art/Characters/" + image_name + ".png")
		
		if image_texture != null:
			image_node.texture = image_texture
		else:
			print("[VisualSystem] Texture not found")
			return
