tool
extends Control
class_name Program


var isReady : bool = false
var background : ColorRect


# Called when the node enters the scene tree for the first time.
func _ready():
	if(!Engine.editor_hint):
		print("Initializing Root...")
		print("Main Thrread # " + "TBD")
		print("user:// data directory: " + OS.get_user_data_dir())

	background = get_node("Background")
	return


# Called every frame. 'delta' is the elapsed time since the previous frame.
#func _process(delta):
#	pass

func _OnThreadButtonPressed():
	print("This is a stub for making a bunch of threaded workers")
	print("Perhaps I will maintain the worker thread code as a C# module")

func _OnAutoThemeChanged():
	pass # stub for now
