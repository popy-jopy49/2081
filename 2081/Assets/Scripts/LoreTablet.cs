using UnityEngine;

public class LoreTablet : MonoBehaviour, IInteractable
{

    public void OnInteract()
    {

        Destroy(gameObject);
    }

}
