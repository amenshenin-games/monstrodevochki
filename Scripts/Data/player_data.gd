extends Node

var data: Dictionary = {
	"player_name": '',
	"level": 0.0,
	"experience": 0.0,
	"points_avaliable": 0.0,
	"current_health": 10.0,
	"money": 10.0,
	"inventory": {},
	"party": [],
	"stats": {
		"strength": 0.0,
		"agility": 0.0,
		"mind": 0.0,
		"personality": 0.0,
		"health": 0.0,
		}
}

func has_key(key: String) -> bool:
	print("[player_data] has_key(", key, "): ", data.has(key))
	return data.has(key)

func get_value(key: String):
	print("[player_data] get_value(", key, "): ", data.get(key, null))
	return data.get(key, null)

func set_value(key: String, value):
	print("[player_data] set_value(", key, ", ", value, ")")
	data[key] = value
