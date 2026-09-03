extends Node


# YARN
# reset 
signal reset(Name: String)

# visual 
signal add_image(Name: String)
signal change_image(Name: String)
signal change_background(Name: String)

# sound 
signal play_sound(Name: String)
signal play_music(Name: String)

# stats 
signal upgrade_stat(Stat: String)
signal get_stat(Stat: String)
signal set_stat(Stat: String, Value: int)

# checks 
signal check(CheckId: String, Difficulty: int, Stat: String, Disadvantage: bool, Advantage: bool)
signal passive_check(CheckId: String, Difficulty: int, Stat: String, Disadvantage: bool, Advantage: bool)

# debug
signal debug_output()

# SYSTEMS
# data
signal save_data()
signal load_data()
