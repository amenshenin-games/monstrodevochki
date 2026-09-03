extends Node

var data = {}

func has_key(key: String) -> bool:
	print("[game_data] has_key(", key, "): ", data.has(key))
	return data.has(key)

func get_value(key: String):
	print("[game_data] get_value(", key, "): ", data.get(key, null))
	return data.get(key, null)

func set_value(key: String, value):
	print("[game_data] set_value(", key, ", ", value, ")")
	data[key] = value
