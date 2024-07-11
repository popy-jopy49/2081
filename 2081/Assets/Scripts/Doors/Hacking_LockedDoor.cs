using UnityEngine;
using SWAssets.Utils;

public class Hacking_LockedDoor : Door
{

	protected override bool OnButtonPress(RaycastHit hitInfo)
	{
		// Have 1 in 3 chance of getting each minigame
		int randomChance = Random.Range(0, 100);

		if (randomChance <= 33)
		{
			// QTE

		}
		else if (randomChance <= 66)
		{
			// Sequence Game

		}
		else
		{
			// Maze Or Sudoku

		}

		return true;
	}

}
