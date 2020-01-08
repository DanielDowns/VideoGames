if(isLeader)
{
	show_debug_message(string(id) + " leader with no patrol...") 	
	/// \todo pick random points to move to or establish boundaries to patrol
}
else
{
	if(leader != 0) //can't follow a dead guy
	{
		var xTarget = leader.x 
		var yTarget = leader.y	
	
		//pick location to the rear of leader and go there
		targetArray[0] = 0
		targetArray[1] = 0
		targetArray = calculateFormationPosition_sc(leader.x, leader.y, leader.direction, self.followPos)
	
		xTarget = targetArray[0]
		yTarget = targetArray[1]
	
		move_towards_waypoint_sc(xTarget, yTarget)
	
	}
	else
	{
		show_debug_message(string(id) + " no leader to follow...") 		
	}
}
