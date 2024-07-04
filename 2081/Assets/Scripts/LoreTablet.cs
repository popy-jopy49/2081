using UnityEngine;

public class LoreTablet : MonoBehaviour, IInteractable
{

    public void OnInteract()
    {
        print("The spacestation was taken over by AI. Get to the control room and press the conveniently placed self destruct button to destroy it and end its rein of terror!");
        Destroy(gameObject);
    }

}
