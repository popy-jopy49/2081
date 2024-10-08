using UnityEngine;

public class InteractUIO : MonoBehaviour
{

    GameObject child;

    private void Awake()
    {
        // Get Actual text object as to not disable this script and set initially to hidden
        child = transform.GetChild(0).gameObject;
        child.SetActive(false);
        // Call function when an interactable goes in or out of range
        Player.OnInteractablesChange += OnInteractableChange;
        print(child.activeSelf);
    }

    private void Update()
    {
        print("Update" + child.activeSelf);
    }

    private void OnInteractableChange(object sender, int count)
    {
        // Hide if there are no interactables in range. Show if there are
        child.SetActive(count > 0);
        print("change" + child.activeSelf);
    }

}
