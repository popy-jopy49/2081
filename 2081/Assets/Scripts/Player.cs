using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{

    public static EventHandler<int> OnSanityKitsUpdated;
    public static EventHandler<(float c, float m)> OnSanityChanged;
    private float sanity = 0;
    [SerializeField] private float maxSanity = 20f;
    [SerializeField] private float maxSanityIncrease = 4f;
    [SerializeField] private bool debug = true;
    private static int sanityKits = 3;

    private void Awake()
    {
        sanity = maxSanity;
        OnSanityChanged?.Invoke(null, (sanity, maxSanity));
        InputManager.MAIN.Character.UseSanityKit.started += UseSanityKit_Started;
    }

    private void Update()
    {
        if (debug)
        {
            sanity = Mathf.Clamp(sanity -= 0.01f, 0, maxSanity);
            OnSanityChanged?.Invoke(null, (sanity, maxSanity));
        }
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

    public void SetMaxSanity(float new_max_sanity)
    {
        maxSanity = new_max_sanity;
        OnSanityChanged?.Invoke(null, (sanity, maxSanity));
    }

    public void AddSanity(float added_sanity)
    {
        sanity += added_sanity;
        OnSanityChanged?.Invoke(null, (sanity, maxSanity));
    }

    public static void AddSanityKit()
    {
        sanityKits++;
        OnSanityKitsUpdated?.Invoke(null, sanityKits);
    }

}
