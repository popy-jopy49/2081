using SWAssets;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : Singleton<Player>
{

    // Events
    public static EventHandler<int> OnInteractablesChange;
    public static EventHandler<int> OnSanityKitsUpdated;
    public static EventHandler<(float c, float m)> OnSanityChanged;

    // Sanity values
    [SerializeField] private float maxSanity = 20f;
    [SerializeField] private float sanityLostOnFailure = 5f;
    [SerializeField] private float sanityWonOnWin = 3f;
    [SerializeField] private float sanityLostAroundLight = 0.5f;
    [SerializeField] private float maxSanityIncrease = 4f;
    [SerializeField] private bool debug = true;
    private static int sanityKits = 3;
	private float sanity = 0;

    // Other
	private Vector3 respawnPos = Vector3.zero;
    private List<IInteractable> interactables = new();
    private static bool hasKeycard = false;

    private void Awake()
    {
        // Set intial values and subscribe to correct events
        sanity = maxSanity;
        OnSanityChanged?.Invoke(null, (sanity, maxSanity));
        InputManager.MAIN.Character.UseSanityKit.started += UseSanityKit_Started;
        InputManager.MAIN.Character.Interact.started += Interact_Started;
        OnSanityChanged += OnSanityChange;
    }

    private void Start()
    {
        PuzzleGrid.Setup(GameAssets.I.MazePrefab, GameAssets.I.MazeFiles, () => print("You Win!"));
    }

    private void Interact_Started(InputAction.CallbackContext obj)
    {
        // If interactables nearby, call OnInteract();
        if (interactables.Count <= 0) return;
        // If interaction successful, remove it from nearby interactables
        if (interactables[0].OnInteract())
		{
			interactables.RemoveAt(0);
            OnInteractablesChange?.Invoke(this, interactables.Count);
            // TODO: Play success sound

            //Add sanity
            AddSanity(sanityWonOnWin);
		}
		else
        {
            // TODO: Play failure sound

            // Remove sanity
            DecreaseSanity(sanityLostOnFailure);
        }
    }

#if UNITY_EDITOR
    private void Update()
    {
        // DEBUG: test to see sanity work and sanity kits
        if (debug)
        {
            sanity = Mathf.Clamp(sanity -= 0.01f, 0, maxSanity);
            OnSanityChanged?.Invoke(null, (sanity, maxSanity));
        }
    }
#endif

    private void OnSanityChange(object sender, (float c, float m) sanity)
    {
        print("Dead");
        if (sanity.c > 0)
            return;
        // Respawn at position if sanity reaches 0
        transform.position = respawnPos;
        this.sanity = maxSanity;
    }

    private void UseSanityKit_Started(InputAction.CallbackContext obj)
    {
        if (sanityKits <= 0) // Only use if we have one
            return;
        sanityKits--;
        // Update UI and values of sanity
        OnSanityKitsUpdated?.Invoke(this, sanityKits);
        SetMaxSanity(maxSanity + maxSanityIncrease);
        AddSanity(maxSanityIncrease);
    }

    // Set max sanity and update UI
    private void SetMaxSanity(float new_max_sanity)
    {
        maxSanity = new_max_sanity;
        OnSanityChanged?.Invoke(null, (sanity, maxSanity));
    }

    // Increase sanity by amount and update UI
    private void AddSanity(float added_sanity)
    {
        sanity += added_sanity;
        OnSanityChanged?.Invoke(null, (sanity, maxSanity));
    }

    private void OnTriggerEnter(Collider other)
    {

        if (!other.TryGetComponent(out IInteractable pickupable)) return;

		// Add interactable to list of nearby interactables and update UI
		interactables.Add(pickupable);
        OnInteractablesChange?.Invoke(this, interactables.Count);
    }

	private void OnTriggerStay(Collider other)
	{
        if (!other.TryGetComponent(out FlashingLight fl))
            return;

        // Decrease sanity for every frame around the flickering light
		DecreaseSanity(sanityLostAroundLight);
	}

	private void OnTriggerExit(Collider other)
    {
        if (!other.TryGetComponent(out IInteractable pickupable)) return;

        // Remove interactable from list of nearby interactables and update UI
        interactables.Remove(pickupable);
        OnInteractablesChange?.Invoke(this, interactables.Count);
    }

    public void DecreaseSanity(float amount)
    {
        // decrease sanity
        sanity -= amount;
        OnSanityChanged?.Invoke(this, (sanity, maxSanity));
    }

    // Increase sanity kit count by 1 and update UI
    public static void AddSanityKit()
    {
        sanityKits++;
        OnSanityKitsUpdated?.Invoke(null, sanityKits);
    }

    // Set respawn position to last checkpoint position
    public void SetLatestCheckpoint(Vector3 pos)
    {
        respawnPos = pos;
    }

    public static void PickUpKeycard() => hasKeycard = true;
    public static bool HasKeycard() => hasKeycard;

}
