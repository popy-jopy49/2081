using UnityEngine;
using UnityEngine.SceneManagement;

public static class SoundManager
{

	private static AudioSource effectsSource;
	private static AudioSource musicSource;

	static SoundManager()
	{
		effectsSource = GameObject.Find("EffectsSource").GetComponent<AudioSource>();
		musicSource = GameObject.Find("MusicSource").GetComponent<AudioSource>();
		SceneManager.activeSceneChanged += BeginMusic;
		BeginMusic(new Scene(), new Scene());
	}

	public static void PlayEffect(GameAssets.Audio audio)
	{
		audio.Play(effectsSource);
	}

	private static void BeginMusic(Scene scene1, Scene scene2)
    {
        GameAssets.I.MenuMusicAudio.Play(musicSource);
        GameAssets.I.LevelMusicAudio.Play(musicSource);
    }

}
