using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterControllerFPS : MonoBehaviour {

    [SerializeField] private CharacterController controller;

    // Movement values
    private float speed = 12f;
    [SerializeField] private float walkSpeed = 12f;
    [SerializeField] private float crouchSpeed = 12f;
    [SerializeField] private float sprintSpeed = 25f;
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private float jumpHeight = 3f;
    private Vector3 moveOffset;

    // Ground Checking
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundDistance = 0.4f;
    [SerializeField] private LayerMask whatIsGround;

    Vector3 velocity;
    bool isGrounded;

    // Energy values
    public static EventHandler<(float c, float m)> OnEnergyChanged;
    private float currentEnergy = 0f;
    [SerializeField] private float maxEnergy = 20f;
    [SerializeField] private float energyRegain = 0.1f;
    [SerializeField] private float jumpEnergy = 3f;
    [SerializeField] private float sprintEnergy = 0.2f;

    private void OnEnable()
    {
        // Update maxEnergy on UI
        currentEnergy = maxEnergy;
        OnEnergyChanged?.Invoke(this, (currentEnergy, maxEnergy));
        speed = walkSpeed;

		// Subscribe from movement input functions
		InputManager.MAIN.Character.Sprint.started += Sprint;
        InputManager.MAIN.Character.Sprint.canceled += Walk;
        InputManager.MAIN.Character.Crouch.started += Crouch;
        InputManager.MAIN.Character.Crouch.canceled += Walk;
    }

    private void OnDisable()
    {
        // Unsubscribe from movement input functions
        InputManager.MAIN.Character.Sprint.started -= Sprint;
        InputManager.MAIN.Character.Sprint.canceled -= Walk;
        InputManager.MAIN.Character.Crouch.started -= Crouch;
        InputManager.MAIN.Character.Crouch.canceled -= Walk;
    }

    private void Crouch(InputAction.CallbackContext ctx)
    {
        // Don't crouch if we are already sprinting
        if (speed != walkSpeed) return;
        speed = crouchSpeed;

		// Change camera pos and collider to give allusion of crouching
		GameValues.GetCamera().transform.localPosition = new Vector3(0f, 0f, 0f);
        controller.height = 2f;
        controller.center = new Vector3(0, -1, 0);
    }

    private void Sprint(InputAction.CallbackContext ctx)
    {
        // If we are not crouching, sprint
        if (speed == walkSpeed) speed = sprintSpeed;
    }

    private void Walk(InputAction.CallbackContext ctx)
    {
        // Bound check to see if we must keep crouching
        if (Physics.Raycast(transform.position, transform.up, 0.99f, whatIsGround))
            return;
        // Reset values for speed and collider
        speed = walkSpeed;
		GameValues.GetCamera().transform.localPosition = new Vector3(0f, 1.55f, 0f);
        controller.height = 4f;
        controller.center = new Vector3(0, 0, 0);
    }

    void Update()
    {
        // Check if player is touching the ground
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, whatIsGround);

        // Set velocity to constant when on the ground
        if(isGrounded && velocity.y < 0)
		{
            velocity.y = -2f;
		}

        // Get movement input
        Vector2 walkInput = InputManager.MAIN.Character.Move.ReadValue<Vector2>().normalized;
        // Set move vectors in correct directions in local space
        Vector3 move = transform.right * walkInput.x + transform.forward * walkInput.y;

        // Jump if on the ground and have enough energy
		if (InputManager.MAIN.Character.Jump.triggered && isGrounded && currentEnergy >= jumpEnergy)
		{
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            currentEnergy -= jumpEnergy;
		}

        Physics.SyncTransforms();
		// Move the player first with movement and then with jump
		controller.Move((speed * Time.deltaTime * move) + moveOffset);
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        // Lose energy when sprinting and moving otherwise gain energy
        if (speed == sprintSpeed && walkInput != Vector2.zero)
            currentEnergy -= sprintEnergy;
        else
            currentEnergy += energyRegain;

        // Clamp energy between 0 and the max
        currentEnergy = Mathf.Clamp(currentEnergy, 0, maxEnergy);

        // Set speed to walking when the player runs out of energy
        if (currentEnergy <= 0)
            speed = walkSpeed;

        // Update UI
        OnEnergyChanged?.Invoke(this, (currentEnergy, maxEnergy));
    }

    public void AddMoveOffset(Vector3 moveDir) => moveOffset = moveDir;

}
