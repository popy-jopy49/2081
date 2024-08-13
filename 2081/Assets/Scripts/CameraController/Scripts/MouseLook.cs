using UnityEngine;

public class MouseLook : MonoBehaviour {

    [SerializeField] private float mouseSensitivity = 5f;
	[SerializeField] private Transform player;
    private float xRotation;

	void Awake()
	{
        // Lock cursor to the screen and hide it
		Cursor.lockState = CursorLockMode.Locked;
	}

	void Update()
    {
        // Get input on mouse movement and modify it based on frame count and sensitivity
        Vector2 mouseInput = InputManager.MAIN.Character.Camera.ReadValue<Vector2>();
        float mouseX = mouseInput.x * mouseSensitivity * Time.deltaTime * 1000f;
        float mouseY = mouseInput.y * mouseSensitivity * Time.deltaTime * 1000f;

        // Rotate player on horizontal mouse movement
        player.Rotate(Vector3.up * mouseX);

        // Rotate camera on the x-axis for vertical mouse movement
        xRotation = Mathf.Clamp(xRotation -= mouseY, -90f, 90f);
        transform.localRotation = Quaternion.Euler(xRotation, 0, 0);
    }
    
}
