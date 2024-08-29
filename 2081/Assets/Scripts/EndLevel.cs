using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(BoxCollider))]
public class EndLevel : MonoBehaviour
{
	[SerializeField] private string SceneName;

    private void OnTriggerEnter(Collider other)
	{
		if (!other.CompareTag("Player"))
			return;

		// Load next level
		//if (SceneManager.GetSceneByBuildIndex(SceneManager.GetActiveScene().buildIndex + 1).IsValid())
		//{
			//SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
		//}
		//else
		//{
			//Cursor.lockState = CursorLockMode.None;
			//SceneManager.LoadScene(0);
		//}

		if (SceneName != null)
		{
			SceneManager.LoadScene(SceneName);
		}
		else
		{
			Cursor.lockState = CursorLockMode.None;
			SceneManager.LoadScene(0);
		}
	}

}
