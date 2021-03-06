extends "res://Entities/Fishing/Fish.gd"

export var max_coordinates:Vector2

const VELOCITY:int = 100

func _process(delta):
	# Move randomly/erratically
	var sign_x = 1
	if randi() % 100 <= 50: sign_x = -1
	
	var sign_y = 1
	if randi() % 100 <= 50: sign_y = -1
	
	self.position.x += (sign_x * VELOCITY * delta)
	self.position.y += (sign_y * VELOCITY * delta)
	
	if self.position.x  < 0:
		self.position.x = 0
	elif self.position.x + $ColorRect.margin_right > max_coordinates.x:
		self.position.x = max_coordinates.x - $ColorRect.margin_right
		
	if self.position.y < 0:
		self.position.y = 0
	elif self.position.y + $ColorRect.margin_bottom > max_coordinates.y:
		self.position.y = max_coordinates.y - $ColorRect.margin_bottom
	
func _on_area_entered(area):
	self.on_fish_body_entered(area)

func _on_area_exited(area):
	self.on_fish_body_exited(area)