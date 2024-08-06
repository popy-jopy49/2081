using UnityEngine;
using UnityEngine.InputSystem.XR;

[RequireComponent(typeof(BoxCollider))]
public class AirLockRoom : MonoBehaviour
{

    [SerializeField] private Vector3 moveDir = Vector3.right;
    [SerializeField] private float speed = 10f;
    private CharacterControllerFPS player;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player = other.GetComponent<CharacterControllerFPS>();
            // Move the player in the move direction
            player.AddMoveOffset(speed * Time.deltaTime * moveDir);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Stop moving the player in the move direction
            player.AddMoveOffset(Vector3.zero);
            player = null;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (!player)
            return;

    }

}
