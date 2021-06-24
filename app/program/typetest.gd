extends ColorRect


export var myColor : Color = Color(1,0,1,1)


# Called when the node enters the scene tree for the first time.
func _ready():
	color = myColor


# Called every frame. 'delta' is the elapsed time since the previous frame.
#func _process(delta):
#	pass
