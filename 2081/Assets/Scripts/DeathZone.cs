using UnityEngine;

[RequireComponent (typeof(BoxCollider))]
public class DeathZone : MonoBehaviour
{
    
    private Player player;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            player = other.GetComponent<Player>();
    }

    private void OnTriggerStay(Collider other)
    {
        if (player)
        {
            player.DecreaseSanity(10);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            player = null;
    }

}
