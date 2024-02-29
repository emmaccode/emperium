extends CharacterBody2D


const SPEED = 300.0
const maxspeed = 450
const JUMP_VELOCITY = -400.0
var left = false
var stun = 0
# Get the gravity from the project settings to be synced with RigidBody nodes.
var gravity = ProjectSettings.get_setting("physics/2d/default_gravity")

func animate(anim:String):
	var fill = !(left)
	$body/armrear.flip_h = fill
	$body.flip_h = fill
	$body/eyes.flip_h = fill
	$body/armfront.flip_h = fill
	$body/hat.flip_h = fill
	$body/mouth.flip_h = fill
	$anims.play(anim)

func _physics_process(delta):
	var jumping = false
	var grounded = is_on_floor()
	var crouching = Input.is_action_pressed("crouch") 
	var anim = "idle"
	if Input.is_action_just_pressed("ui_accept") and grounded and stun < 1:
		velocity.y = JUMP_VELOCITY
	# Get the input direction and handle the movement/deceleration.
	# As good practice, you should replace UI actions with custom gameplay actions.
	var direction = Input.get_axis("ui_left", "ui_right")
	if (direction == -1 and left == true) or (direction == 1 and left == false):
		left = !(left)
	if direction && stun < 2:
		anim = "run"
		if velocity.x > 250 && velocity.x < maxspeed:
			velocity.x += 50 * direction
		else:
			velocity.x = direction * SPEED
	else:
		velocity.x = move_toward(velocity.x, 0, SPEED)
		if velocity.x > 30:
			anim = "slowing"
		else:
			anim = "idle"
	if stun > 3:
		pass
	elif stun > 1:
		pass
	elif stun == 1:
		pass
	if not grounded:
		velocity.y += gravity * delta
		anim = "falling"
	if crouching && not grounded:
		anim = "fallcrouching"
	elif crouching:
		anim = "crouching"
	animate(anim)
	move_and_slide()
