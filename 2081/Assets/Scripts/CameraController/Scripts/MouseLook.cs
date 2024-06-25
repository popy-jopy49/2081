using UnityEngine;

public class MouseLook : MonoBehaviour {

    public float mouseSensitivity = 1000f;
    public Transform player;

    private float xRotation = 0f;

	void Start()
	{
		Cursor.lockState = CursorLockMode.Locked;
	}

	void Update()
    {
        Vector2 mouseInput = InputManager.MAIN.Character.Camera.ReadValue<Vector2>();
        float mouseX = mouseInput.x * mouseSensitivity * Time.deltaTime;
        float mouseY = mouseInput.y * mouseSensitivity * Time.deltaTime;
        player.Rotate(Vector3.up * mouseX);
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        transform.localRotation = Quaternion.Euler(xRotation, 0, 0);
    }
    
}
