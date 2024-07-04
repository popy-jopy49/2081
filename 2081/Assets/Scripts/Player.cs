using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{

    public static EventHandler<int> OnInteractablesChange;
    public static EventHandler<int> OnSanityKitsUpdated;
    public static EventHandler<(float c, float m)> OnSanityChanged;
    private float sanity = 0;
    [SerializeField] private float maxSanity = 20f;
    [SerializeField] private float maxSanityIncrease = 4f;
    [SerializeField] private bool debug = true;
    private static int sanityKits = 3;
    private static Vector3 respawnPos = Vector3.zero;
    private List<IInteractable> interactables = new();

    private void Awake()
    {
        sanity = maxSanity;
        OnSanityChanged?.Invoke(null, (sanity, maxSanity));
        InputManager.MAIN.Character.UseSanityKit.started += UseSanityKit_Started;
        InputManager.MAIN.Character.Interact.started += Interact_Started;
        OnSanityChanged += OnSanityChange;
    }

    private void Interact_Started(InputAction.CallbackContext obj)
    {
        if (interactables.Count <= 0) return;
        interactables[0].OnInteract();
        interactables.RemoveAt(0);
    }

    private void Update()
    {
        if (debug)
        {
            sanity = Mathf.Clamp(sanity -= 0.01f, 0, maxSanity);
            OnSanityChanged?.Invoke(null, (sanity, maxSanity));
        }
    }

    private void OnSanityChange(object sender, (float c, float m) sanity)
    {
        if (sanity.c > 0)
            return;
        // Respawn
        transform.position = respawnPos;
    }

    private void UseSanityKit_Started(InputAction.CallbackContext obj)
    {
        if (sanityKits <= 0)
            return;
        sanityKits--;
        OnSanityKitsUpdated?.Invoke(this, sanityKits);
        SetMaxSanity(maxSanity + maxSanityIncrease);
        AddSanity(maxSanityIncrease);
    }

    private void SetMaxSanity(float new_max_sanity)
    {
        maxSanity = new_max_sanity;
        OnSanityChanged?.Invoke(null, (sanity, maxSanity));
    }

    private void AddSanity(float added_sanity)
    {
        sanity += added_sanity;
        OnSanityChanged?.Invoke(null, (sanity, maxSanity));
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.TryGetComponent(out IInteractable pickupable)) return;

        // Add
        interactables.Add(pickupable);
        OnInteractablesChange?.Invoke(this, interactables.Count);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.TryGetComponent(out IInteractable pickupable)) return;

        // Remove
        interactables.Remove(pickupable);
        OnInteractablesChange?.Invoke(this, interactables.Count);
    }

    public static void AddSanityKit()
    {
        sanityKits++;
        OnSanityKitsUpdated?.Invoke(null, sanityKits);
    }

    public static void SetLatestCheckpoint(Vector3 pos)
    {
        respawnPos = pos;
    }

}
