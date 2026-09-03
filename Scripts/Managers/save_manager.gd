extends Node

var save_path: String = "user://savegame.json"

func _ready() -> void:
	PlayerData.data.level = 1
	SignalBus.connect("debug_output", debug_output)
	SignalBus.connect("save_data", save_data)
	SignalBus.connect("load_data", load_data)

func load_data() -> void:
	var save_file = FileAccess.open(save_path, FileAccess.READ)
	if save_file == null:
		return

	var json_text = save_file.get_as_text()
	var json = JSON.parse_string(json_text)
	if json == null:
		return

	PlayerData.data = json.get("player_data", {})
	GameData.data   = json.get("game_data", {})


func save_data() -> void:
	var save_file = FileAccess.open(save_path, FileAccess.WRITE)
	var data = {"player_data": PlayerData.data, "game_data": GameData.data}
	var json_string = JSON.stringify(data)
	save_file.store_line(json_string)

func debug_output() -> void:
	print("======[SAVE MANAGER]======")
	print("___ PLAYER DATA ___")
	print(PlayerData.data)
	print("___ GAME DATA ___")
	print(GameData.data)
	print("======[SAVE MANAGER]======")
