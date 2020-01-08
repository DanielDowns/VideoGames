/// all behaviors are run here
/// can only have 1 instance of a given behavior at a time in the order queue
/// higher values are more important


if(isLeader) //insert behavior for testing, add in leader's "mind" to discern when to order
	ds_priority_add(behaviorQueue, "cover", 2) 

behavior = ds_priority_find_max(behaviorQueue)


var isDone = false
switch (behavior){
	case "patrol":
	{
		patrol_sc()
		show_debug_message(string(id) +" follow action called")
		break
	}
	case "flank":
	{
		/// \todo there needs to be a way to store info the target to attac
		show_debug_message(string(id) +" flank action called")	
	
		var player = instance_find(playerShip, 0)
		isDone = flank_sc(player)
		break
	}
	case "cover":
	{
		show_debug_message(string(id) +" cover action called")	
		
		var player = instance_find(playerShip, 0)
		isDone = cover_sc(player)
		break
	}
}

if(isDone)
{
	ds_priority_delete_value(behaviorQueue, behavior);
}