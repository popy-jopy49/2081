using TMPro;
using UnityEngine;

public class KeyPad_LockedDoor : Door
{

	[SerializeField] private string digitCode = "1111";
	private string currentCode = "";
	private TMP_Text digitDisplay;

	protected override void Awake()
	{
		// Call base awake and find digit UI
		base.Awake();
		digitDisplay = transform.Find("Screen").Find("Digits").GetComponent<TMP_Text>();
		SetCode("");
	}

	protected override bool OnButtonPress(RaycastHit hitInfo)
	{
		// On Tick pressed, check against code
		if (hitInfo.transform.name == "Tick")
		{
			// If player has the incorrect code, the interaction failed
			if (currentCode != digitCode)
			{
				SetCode("");
				return false;
			}

			// Else, open the door and destroy script to prevent second interaction
			OpenDoor();
			Destroy(this);
			return true;
		}

		// On X pressed, remove entire code. Return false because we can still interact
		if (hitInfo.transform.name == "X")
		{
			SetCode("");
			return false;
		}

		// If length of code is greater than or equal to 4, don't allow more digits
		if (currentCode.Length >= 4)
		{
			return false;
		}

		// On number pressed, add it to the string
		SetCode(currentCode + hitInfo.transform.name);
		return false;
	}

	private void SetCode(string text)
	{
		currentCode = text;
		digitDisplay.text = currentCode;
	}

}
