using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterControllerFPS : MonoBehaviour {

    [SerializeField] private CharacterController controller;

    private float speed = 12f;
    [SerializeField] private float walkSpeed = 12f;
    [SerializeField] private float crouchSpeed = 12f;
    [SerializeField] private float sprintSpeed = 25f;
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private float jumpHeight = 3f;
    private Transform cameraO;

    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundDistance = 0.4f;
    [SerializeField] private LayerMask whatIsGround;

    Vector3 velocity;
    bool isGrounded;

    public static EventHandler<(float c, float m)> OnEnergyChanged;
    private float currentEnergy = 0f;
    [SerializeField] private float maxEnergy = 20f;
    [SerializeField] private float energyRegain = 0.1f;
    [SerializeField] private float jumpEnergy = 3f;
    [SerializeField] private float sprintEnergy = 0.2f;

    private void Awake()
    {
        cameraO = transform.Find("Main Camera");
    }

    private void OnEnable()
    {
        currentEnergy = maxEnergy;
        OnEnergyChanged?.Invoke(this, (currentEnergy, maxEnergy));
        speed = walkSpeed;
        InputManager.MAIN.Character.Sprint.started += Sprint;
        InputManager.MAIN.Character.Sprint.canceled += Walk;
        InputManager.MAIN.Character.Crouch.started += Crouch;
        InputManager.MAIN.Character.Crouch.canceled += Walk;
    }

    private void OnDisable()
    {
        InputManager.MAIN.Character.Sprint.started -= Sprint;
        InputManager.MAIN.Character.Sprint.canceled -= Walk;
        InputManager.MAIN.Character.Crouch.started -= Crouch;
        InputManager.MAIN.Character.Crouch.canceled -= Walk;
    }

    private void Crouch(InputAction.CallbackContext ctx)
    {
        if (speed != walkSpeed) return;
        speed = crouchSpeed;
        cameraO.transform.localPosition = new Vector3(0f, 0f, 0f);
        controller.height = 2f;
        controller.center = new Vector3(0, -1, 0);
    }
    private void Sprint(InputAction.CallbackContext ctx)
    {
        if (speed == walkSpeed) speed = sprintSpeed;
    }
    private void Walk(InputAction.CallbackContext ctx)
    {
        speed = walkSpeed;
        cameraO.transform.localPosition = new Vector3(0f, 1.55f, 0f);
        controller.height = 4f;
        controller.center = new Vector3(0, 0, 0);
    }

    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, whatIsGround);

        if(isGrounded && velocity.y < 0)
		{
            velocity.y = -2f;
		}

        Vector2 walkInput = InputManager.MAIN.Character.Move.ReadValue<Vector2>().normalized;

        Vector3 move = transform.right * walkInput.x + transform.forward * walkInput.y;

		if (InputManager.MAIN.Character.Jump.triggered && isGrounded && currentEnergy >= jumpEnergy)
		{
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            currentEnergy -= jumpEnergy;
		}

        controller.Move(speed * Time.deltaTime * move);
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        if (speed == sprintSpeed && walkInput != Vector2.zero)
            currentEnergy -= sprintEnergy;
        else
            currentEnergy += energyRegain;

        currentEnergy = Mathf.Clamp(currentEnergy, 0, maxEnergy);

        if (currentEnergy <= 0)
            speed = walkSpeed;

        OnEnergyChanged?.Invoke(this, (currentEnergy, maxEnergy));
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.TryGetComponent(out IPickUpAble pickupable)) return;

        pickupable.OnPickUp();
        Destroy(other.gameObject);
    }

}
