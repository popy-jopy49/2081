using UnityEngine;

public class CloningMachine : MonoBehaviour, IInteractable
{

    public void OnInteract()
    {
        // Set checkpoint of player to a little bit in front of the checkpoint
        Player.SetLatestCheckpoint(transform.position + new Vector3(2f, 0f, 0f));
        // Destroy script so that this can't be interacted with again
        Destroy(this);
    }

}
