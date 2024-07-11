using UnityEngine;

public class Door : MonoBehaviour, IInteractable
{

	[SerializeField] protected LayerMask doorButton;

	protected Animator anim;

	protected virtual void Awake()
	{
		// Get Animation Component on door
		anim = GetComponent<Animator>();
	}

	protected virtual void OpenDoor()
	{
		// Activate transition from closed door to open door animations
		anim.SetTrigger("OpenDoor");
	}

	public bool OnInteract()
	{
		// Cast ray from player's centre
		Vector3 pos = Player.CAMERA.ScreenToWorldPoint(Vector3.zero);

		// If that ray hits a button and it's parent is this door, call button press function
		if (!Physics.Raycast(pos, Player.CAMERA.transform.forward, out RaycastHit hitInfo, 10f, doorButton))
			return false;

		if (hitInfo.transform.parent != transform)
			return false;

		return OnButtonPress(hitInfo);
	}

	protected virtual bool OnButtonPress(RaycastHit hitInfo)
	{
		// By default, open the door and destroy this script to prevent second interaction
		OpenDoor();
		Destroy(this);
		return true;
	}

}
