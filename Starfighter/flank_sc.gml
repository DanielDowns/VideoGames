//attack the target from the side, then come back

target = argument0 //target to attack

var COMPLETE_RANGE = 50
var SPEED_MAX_POWER = 100
var FLANK_OFFSET = 100

// if its dead, we're done
if(!instance_exists(target))
{
	path_delete(flankPath)
	flankPath = -1
	return true
}

//calculate rotation to figure out proportion

if(flankPath == -1)
{
	flankPath = path_add()
	global.testPath = flankPath
	path_set_closed(flankPath, false); //don't loop
	path_set_kind(flankPath, 1) //make path kinda smooth
	path_set_precision(flankPath, 4)
	
	/// \todo add smoothing based on ship movement limits
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
	var xFactor = cos(target.direction * (3.1415/180)) 
	var yFactor = sin(target.direction * (3.1415/180)) * -1
	
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
	xOffset += (cos(leftAngle * (3.1415/180)) * FLANK_OFFSET)
	yOffset += (sin(leftAngle * (3.1415/180)) * -1 * FLANK_OFFSET)
	
	var flankX = target.x + xOffset
	var flankY = target.y + yOffset	
	
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

// if we're close enough to target, assume we attacked and we're done
if distance_to_object(target) < COMPLETE_RANGE
{
	path_end()
	path_delete(flankPath)
	flankPath = -1

	return true
}
	
global.drawFlag = 1
return false

