using UnityEngine;
using UnityEngine.SceneManagement;

public static class SoundManager
{

	private static AudioSource effects;
	private static AudioSource music;

	static SoundManager()
	{
		effects = GameObject.Find("EffectsSource").GetComponent<AudioSource>();
		music = GameObject.Find("MusicSource").GetComponent<AudioSource>();
		SceneManager.activeSceneChanged += BeginMusic;
	}

	public static void PlayEffect(GameAssets.Audio audio)
	{
		audio.Play(effects);
	}

	private static void BeginMusic(Scene scene1, Scene scene2)
    {
        GameAssets.I.MenuMusicAudio.Play(music);
        GameAssets.I.LevelMusicAudio.Play(music);
    }

}
