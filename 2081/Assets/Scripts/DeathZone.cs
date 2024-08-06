using UnityEngine;

public class DeathZone : MonoBehaviour
{

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<Player>().DecreaseSanity(10);
        }
    }

}
