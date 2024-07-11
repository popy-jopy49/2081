using UnityEngine;
using UnityEngine.InputSystem;

public class QTE : MonoBehaviour
{

	private void Awake()
	{
		InputManager.MAIN.Character.Interact.started += OnPress;
	}

	private void OnPress(InputAction.CallbackContext obj)
	{
		// Check if inside zone


		// Move onto next zone or complete event
	}

}
