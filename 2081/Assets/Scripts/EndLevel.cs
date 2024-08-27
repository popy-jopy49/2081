using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(BoxCollider))]
public class EndLevel : MonoBehaviour
{

	private void OnTriggerEnter(Collider other)
	{
		if (!other.CompareTag("Player"))
			return;

		// Load next level if not level 3
		if (GameValues.GetActiveBuildIndex() != 3)
		{
			SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
		}
		else
		{
			Cursor.lockState = CursorLockMode.None;
			SceneManager.LoadScene(0);
		}
	}

}
