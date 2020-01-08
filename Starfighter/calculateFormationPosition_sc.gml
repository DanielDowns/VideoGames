/// \todo expand to allow more followers, only supports 2 right now
var leaderPositionX = argument0
var leaderPositionY = argument1
var leaderAngle = argument2
var wingPosition = argument3

var offsetSize = 50;

//negative directions are stupid
if(leaderAngle < 0)
	leaderAngle += 360

var xFactor = cos(leaderAngle * (3.1415/180)) * -1 //this is inverted to start with
var yFactor = sin(leaderAngle * (3.1415/180))

var xOffset = (xFactor * offsetSize)
var yOffset = (yFactor * offsetSize)

var wingOffset = 0
if(wingPosition == 0) 
	wingOffset = 90
else wingOffset = -90

var leftAngle = leaderAngle + wingOffset
xOffset += (cos(leftAngle * (3.1415/180)) * -1 * offsetSize)
yOffset += (sin(leftAngle * (3.1415/180)) * offsetSize)

retArray[0] = xOffset + leaderPositionX
retArray[1] = yOffset + leaderPositionY
return retArray