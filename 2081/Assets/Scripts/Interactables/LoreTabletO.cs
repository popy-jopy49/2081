using System;
using UnityEngine;

public class LoreTabletO : MonoBehaviour, IInteractable
{

    [SerializeField] private int logNumber = 0;
    public EventHandler<int> OnLorePickUp;

    // Display lore to the player on pickup
    public bool OnInteract()
    {
        //print("The spacestation was taken over by AI. Get to the control room and press the conveniently placed self destruct button to destroy it and end its rein of terror!");
        // Trigger event that this log should be available
        OnLorePickUp?.Invoke(this, logNumber);
        Destroy(gameObject);
        return true;
    }

}
