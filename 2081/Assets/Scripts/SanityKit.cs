using UnityEngine;

public class SanityKit : MonoBehaviour, IInteractable
{

    // Add a sanity kit to the player's count on pickup
    public void OnInteract()
    {
        Player.AddSanityKit();
        Destroy(gameObject);
    }

}
