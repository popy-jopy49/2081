using UnityEngine;

public class Hacking_LockedDoor : Door
{

	private Transform hackingParent;
	[SerializeField] private bool debug = true;
	[SerializeField] private int debugChance = 50;

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
		if (debug)
			randomChance = debugChance;

		// Chance <= 33: Spawn Quick time event puzzle
		// Chance <= 66: Spawn sequence puzzle
		// Otherwise: spawn maze or sudoky puzzle and make hacking parent the parent
		GameObject puzzle = Instantiate(randomChance <= 50 ? GameAssets.I.QTEPrefab :
					(randomChance <= 100 ? GameAssets.I.SequencePrefab :
					GameAssets.I.MazeOrSudokuPrefab), 
					hackingParent);

		puzzle.GetComponent<HackPuzzle>().OnPuzzleComplete += (_, success) => {
			// Always destroy the puzzle and relock the cursor
			Destroy(puzzle);
			Cursor.lockState = CursorLockMode.Locked;
			// Return if player failed the puzzle
			if (!success)
				return;

			// When the puzzle is successfully completed, open the door and prevent second interaction
			OpenDoor();
			Destroy(this);
			// TODO: Play sound after puzzle completion
		};

		return true;
	}

}
