using UnityEngine;

public class SanityKit : MonoBehaviour, IInteractable
{

    public void OnInteract()
    {
        Player.AddSanityKit();
        Destroy(gameObject);
    }

}
