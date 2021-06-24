extends Panel
class_name MenuPanel

const Program = preload("res://app/program/Program.gd")
const SceneHandler = preload("res://app/program/SceneHandler.cs")
const Utilities = preload("res://library/Utilities.cs")

var main : Program
var sceneHandler : SceneHandler
var myMenu : MenuButton

# Called when the node enters the scene tree for the first time.
func _ready():
	main = owner as Program
	sceneHandler = main.get_node("SceneHandler")

	myMenu = get_node("MenuHBox/MenuButton")
	var myPopup : PopupMenu = myMenu.get_popup()
	myPopup.connect("id_pressed", self, "_OnMenuPopupPressed")
	pass # Replace with function body.


# Called every frame. 'delta' is the elapsed time since the previous frame.
#func _process(delta):
#	pass

func _OnMenuPopupPressed(id : int):
	if (id == 2):
		sceneHandler.SwitchToScene(0)
	elif (id == 3):
		sceneHandler.SwitchToScene(1)

func _OnPrintMenuChildren():
	Utilities.PrintChildrenRecursive(myMenu)
	
