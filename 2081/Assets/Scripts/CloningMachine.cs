using UnityEngine;

public class CloningMachine : MonoBehaviour, IInteractable
{

    bool interactable = true;

    public void OnInteract()
    {
        if (!interactable) return;
        interactable = false;
        Player.SetLatestCheckpoint(transform.position + new Vector3(2f, 0f, 0f));
    }

}
