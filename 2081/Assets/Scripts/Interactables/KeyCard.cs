using UnityEngine;

public class KeyCard : MonoBehaviour, IInteractable
{

	public bool OnInteract()
	{
		Player.PickUpKeycard();
		Destroy(gameObject);
		return true;
	}

}
