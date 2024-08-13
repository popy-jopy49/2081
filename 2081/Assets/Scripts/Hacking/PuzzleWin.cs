using System;
using UnityEngine;

public class PuzzleWin : MonoBehaviour
{

    private PuzzleGrid grid;

    // When colliding with puzzle, win
    private void OnTriggerEnter2D(Collider2D collision)
	{
        if (collision.gameObject.layer != LayerMask.NameToLayer("Puzzle"))
            return;

        // Win
        //winFunc();
    }

    // Sets win func to be called when we win
    public void SetWinFunc(PuzzleGrid grid) => this.grid = grid;

}
