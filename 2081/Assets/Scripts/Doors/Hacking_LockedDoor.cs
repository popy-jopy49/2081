using UnityEngine;

public class Hacking_LockedDoor : Door
{

	private Transform hackingParent;

	protected override void Awake()
	{
		// Do base awake then find the hacking parent
		base.Awake();
		hackingParent = GameValues.GetCanvas().Find("HackingGameParent");
	}

	protected override bool OnButtonPress(RaycastHit hitInfo)
	{
		// Disable script to prevent interaction while playing the puzzle
		enabled = true;

		// Have 1 in 3 chance of getting each minigame
		int randomChance = Random.Range(0, 100);
		// Unlock cursor so player can use it
		Cursor.lockState = CursorLockMode.None;

		// Chance <= 33: Spawn Quick time event puzzle
		// Chance <= 66: Spawn sequence puzzle
		// Otherwise: spawn maze or sudoky puzzle and make hacking parent the parent
		GameObject puzzle = Instantiate(randomChance <= 33 ? GameAssets.I.QTEPrefab :
					(randomChance <= 66 ? GameAssets.I.SequencePrefab :
					GameAssets.I.MazeOrSudokuPrefab), 
					hackingParent);

		// When the puzzle is complete, open the door and prevent second interaction
		puzzle.GetComponent<HackPuzzle>().OnPuzzleComplete += (_, _) => {
			// Relock cursor to centre of screen and hide it
			Cursor.lockState = CursorLockMode.Locked;
			OpenDoor();
			Destroy(this);
			// TODO: Play sound after puzzle completion
		};

		return true;
	}

}
