using System;
using UnityEngine;

public class Player : MonoBehaviour
{

    public static EventHandler<(float c, float m)> OnSanityChanged;
    private static float sanity = 0;
    private static float maxSanity = 0;
    [SerializeField] private float _maxSanity = 20f;

    private void Awake()
    {
        maxSanity = _maxSanity;
        sanity = _maxSanity;
    }

    public static void SetMaxSanity(float new_max_sanity)
    {
        maxSanity = new_max_sanity;
        OnSanityChanged?.Invoke(null, (sanity, maxSanity));
    }

    public static void AddSanity(float added_sanity)
    {
        sanity += added_sanity;
        OnSanityChanged?.Invoke(null, (sanity, maxSanity));
    }

}
