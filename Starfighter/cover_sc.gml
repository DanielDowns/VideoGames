//fly behind and hit someone attacking the leader

/// \todo this is a very similar to flank. What can be consolidated into one function?

target = argument0 //target to attack

var COMPLETE_RANGE = 500 //finish if target is far enough away from leader. Assume it broke off.
var ATTACK_RANGE = 50
var SPEED_MAX_POWER = 100
var FLANK_OFFSET = 100

// if its dead, we're done
if(!instance_exists(target) or point_distance(target.x, target.y, leader.x, leader.y) > COMPLETE_RANGE)
{
	if(flankPath != -1)
		path_delete(flankPath)
	hasFlanked = false //reset so it can flank again
	flankPath = -1
	return true
}


if(flankPath == -1 and hasFlanked == false)
{
	flankPath = path_add()
	global.testPath = flankPath
	path_set_closed(flankPath, false); //don't loop
	path_set_kind(flankPath, 1) //make path kinda smooth
	path_set_precision(flankPath, 4)
	
	//first point is current location
	path_add_point(flankPath, x, y, SPEED_MAX_POWER)	
	
	
	//second point is directly in front to smoothly transition into it
	/// \todo this math is repeated a lot, refactor into function
	var xFactor = cos(direction * (3.1415/180)) 
	var yFactor = sin(direction * (3.1415/180)) * -1
	
	var xVal = x + (50 * xFactor)
	var yVal = y + (50 * yFactor)

	path_add_point(flankPath, xVal, yVal, SPEED_MAX_POWER)
	
	
	//third point is off to side to encourage differnt attack vector
	var xFactor = cos(target.direction * (3.1415/180)) * -1
	var yFactor = sin(target.direction * (3.1415/180)) 
	
	var xOffset = (xFactor * FLANK_OFFSET)
	var yOffset = (yFactor * FLANK_OFFSET)
	
	show_debug_message("x/y offset")
	show_debug_message(xOffset)
	show_debug_message(yOffset)

	/// \todo call randomize() at start to mix up seed
	var wingOffset = 0;
	var attackLeft = (irandom(1) == 0) //sometime attack left, sometimes right
	if(attackLeft == false) 
		wingOffset = 90
	else wingOffset = -90
	
	var leftAngle = target.direction + wingOffset
	xOffset += (cos(leftAngle * (3.1415/180)) * -1 * FLANK_OFFSET)
	yOffset += (sin(leftAngle * (3.1415/180)) * FLANK_OFFSET)
	
	var flankX = target.x + xOffset
	var flankY = target.y + yOffset	
	
	global.testX = flankX
	global.testY = flankY
	
	path_add_point(flankPath, flankX, flankY, SPEED_MAX_POWER)
	
	//fourth point is target's location
	path_add_point(flankPath, target.x, target.y, SPEED_MAX_POWER)
	
	//start following
	path_start(flankPath, 2, path_action_stop, true)	
}

image_angle = direction
//update path based on new target end point
path_change_point(flankPath, 3, target.x, target.y, SPEED_MAX_POWER)

/// \todo afterburner here?
/// \todo fire at enemy!

global.drawFlag = 1
// if we're close enough to target, stop worrying about pathing, and just shoot til its dead
if (distance_to_object(target) < ATTACK_RANGE or hasFlanked == true)
{	
	move_towards_waypoint_sc(target.x, target.y)
	global.drawFlag = 0
	hasFlanked = true
	
	if(flankPath == -1)
	{
		path_end()
		path_delete(flankPath)	
		flankPath = -1
	}
}

image_angle = direction


return false