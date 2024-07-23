using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using SWAssets.Utils;

public class Sequence : HackPuzzle
{

	private Image[] buttons;
	private string sequence = "";
	private int index = 0;
	private bool showingSequence = true;

    private const float FULL_A = 1f;
    private const float FADED_A = 100f / 255f;
    private const int ONE_SECOND = 1000;

    private void Awake()
	{
		buttons = transform.Find("Buttons").GetComponentsInChildren<Image>();
		// Generate random sequence
		int numOfSequences = Random.Range(GameValues.I.minNumberOfSequences, GameValues.I.maxNumberOfSequences);
		for (int i = 0; i < numOfSequences; i++)
		{
			sequence += Random.Range(0, buttons.Length);
		}
		// Show sequence to player
		ShowSequence();
	}

	private async void ShowSequence()
	{
		// Wait a second
		await Task.Delay(ONE_SECOND);
		
		// Loop over each item in the sequence
		for (int i = 0;	i < sequence.Length; i++)
		{
			int buttonIDX = Parse.Int(sequence[i].ToString());
			// Set image to lighter colour
			Color col = buttons[buttonIDX].color;
			col.a = FULL_A;
			buttons[buttonIDX].color = col;
			// TODO: Play sequence sound

			// Wait 1 second
			await Task.Delay(ONE_SECOND);

			// Set image to regular colour
			col.a = FADED_A;
			buttons[buttonIDX].color = col;

			//Wait 1 second
			await Task.Delay(ONE_SECOND);
		}

		showingSequence = false;
	}

	public void ButtonPressed(int childIndex)
	{
		// exit if still showing sequence
		if (showingSequence)
			return;

		// Check if correct button pressed
		if (Parse.Int(sequence[index].ToString()) != childIndex)
		{
			// Fail puzzle
			OnPuzzleComplete?.Invoke(this, false);
			return;
		}

		// Correct button pressed so look at next index in sequence
		index++;
		if (index >= sequence.Length)
		{
			// Successful completion of puzzle
			OnPuzzleComplete?.Invoke(this, true);
		}
	}

}
