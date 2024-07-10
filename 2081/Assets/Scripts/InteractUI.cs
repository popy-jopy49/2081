using UnityEngine;

public class InteractUI : MonoBehaviour
{

    private void Awake()
    {
        // Call function when an interactable goes in or out of range
        Player.OnInteractablesChange += OnInteractableChange;
    }

    private void OnInteractableChange(object sender, int count)
    {
        // Hide if there are no interactables in range. Show if there are
        gameObject.SetActive(count > 0);
    }

}
