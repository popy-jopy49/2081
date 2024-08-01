using UnityEngine;

public class SanityKitO : MonoBehaviour, IInteractable
{

    // Add a sanity kit to the player's count on pickup
    public bool OnInteract()
    {
        Player.AddSanityKit();
        Destroy(gameObject);
        return true;
    }

}
