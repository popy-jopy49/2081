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
        Cursor.lockState = GameValues.I.MenuCursorState;
        if (GameValues.I.MenuCursorVisibility == GameValues.CursorVisibility.Visible) { Cursor.visible = true; }
        else if (GameValues.I.MenuCursorVisibility != GameValues.CursorVisibility.Default) { Cursor.visible = false; }

        GameValues.IN_PUZZLE = true;

        if (debug)
			randomChance = debugChance;

		// Chance <= 33: Spawn Quick time event puzzle
		// Chance <= 66: Spawn sequence puzzle
		// Otherwise: spawn maze and make hacking parent the parent
		Transform puzzle = Instantiate(randomChance <= 33 ? GameAssets.I.QTEPrefab :
					(randomChance <= 66 ? GameAssets.I.SequencePrefab :
					GameAssets.I.MazePrefab), 
					hackingParent);

		hackingParent.GetComponentInChildren<HackPuzzle>().OnPuzzleComplete += (_, success) => {
            // Always destroy the puzzle and relock the cursor
            GameValues.IN_PUZZLE = false;
            Destroy(puzzle.gameObject);
			Cursor.lockState = GameValues.I.GameCursorState;
            if (GameValues.I.GameCursorVisibility == GameValues.CursorVisibility.Visible) { Cursor.visible = true; }
            else if (GameValues.I.GameCursorVisibility != GameValues.CursorVisibility.Default) { Cursor.visible = false; }
            // Return if player failed the puzzle
            if (!success)
			{

                return;
            }

			// When the puzzle is successfully completed, open the door and prevent second interaction
			OpenDoor();
			Destroy(this);
			// TODO: Play sound after puzzle completion
		};

		return true;
	}

}
