using UnityEngine;

public class Card_LockedDoor : Door
{

	protected override bool OnButtonPress(RaycastHit hitInfo)
	{
		// If player does not have keycard, the interaction failed
		if (!Player.HasKeycard())
			return false;

		// Else, open the door and destroy script to prevent second interaction
		OpenDoor();
		Destroy(this);
		return true;
	}

}
