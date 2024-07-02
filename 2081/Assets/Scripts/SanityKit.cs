using UnityEngine;

public class SanityKit : MonoBehaviour, IPickUpAble
{

    public void OnPickUp()
    {
        Player.AddSanityKit();
        Destroy(gameObject);
    }

}
