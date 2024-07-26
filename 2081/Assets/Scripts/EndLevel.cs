using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(BoxCollider))]
public class EndLevel : MonoBehaviour
{

	private void OnTriggerEnter(Collider other)
	{
		if (!other.CompareTag("Player"))
			return;

		// Load next level
		Cursor.lockState = CursorLockMode.None;
		SceneManager.LoadScene(0);//SceneManager.GetActiveScene().buildIndex + 1);
	}

}
