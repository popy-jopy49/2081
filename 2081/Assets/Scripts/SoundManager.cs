using UnityEngine;

public class SoundManager : MonoBehaviour
{

	AudioSource effects;
	AudioSource music;

	private void Awake()
	{
		effects = transform.Find("Effects").GetComponent<AudioSource>();
		music = transform.Find("Music").GetComponent<AudioSource>();
	}

	public void PlayEffect()
	{

	}

	private void BeginMusic()
	{

	}

}
