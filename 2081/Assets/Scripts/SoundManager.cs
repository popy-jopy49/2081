using Michsky.UI.Heat;
using SWAssets;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class SoundManager : Singleton<SoundManager>
{

    [Header("Sound")]
    public AudioMixer MainMixer;
    public Audio SuccessSoundEffect;
    public Audio FailureSoundEffect;
    public Audio MenuMusicAudio;
    public Audio LevelMusicAudio;

    private AudioSource effectsSource;
	private AudioSource musicSource;

	void Awake()
	{
		effectsSource = transform.Find("EffectsSource").GetComponent<AudioSource>();
		musicSource = transform.Find("MusicSource").GetComponent<AudioSource>();
		SceneManager.activeSceneChanged += BeginMusic;
		Debug.Log("Calling");
		BeginMusic(new Scene(), new Scene());
	}

	public void PlayEffect(Audio audio)
	{
		audio.Play(effectsSource);
	}

	private void BeginMusic(Scene scene1, Scene scene2)
    {
		Debug.Log("Begginning Music");
        MenuMusicAudio.Play(musicSource);
        LevelMusicAudio.Play(musicSource);
    }

    public void ChangeMasterVolume(float vol) => MainMixer.SetFloat("Master", vol - 80f);
    public void ChangeSFXVolume(float vol) => MainMixer.SetFloat("SFX", vol - 80f);
    public void ChangeUIVolume(float vol) => MainMixer.SetFloat("UI", vol - 80f);
    public void ChangeMusicVolume(float vol) => MainMixer.SetFloat("Music", vol - 80f);

    private void Update()
    {
        SuccessSoundEffect.Update();
        FailureSoundEffect.Update();
        MenuMusicAudio.Update();
        LevelMusicAudio.Update();
    }

    [System.Serializable]
    public struct Audio
    {
        // the clips themsleves
        public AudioClip[] clips;
        // the scene it should play in
        public bool playInMainMenu;
        // whether they should repeat
        public bool shouldRepeat;
        // Whether they should shuffle
        public bool shouldShuffle;
        // Whether they should play in the pause menu
        public bool playWhilePaused;
        int indexOfTrack;
        AudioSource source;
        bool shouldPlay;

        public void Play(AudioSource source)
        {
            if ((GameValues.GetActiveBuildIndex() != 0 && playInMainMenu) || (!shouldRepeat && indexOfTrack >= clips.Length))
            {
                shouldPlay = false;
                return;
            }
            this.source = source;

            // In the correct scene
            if (shouldShuffle && clips.Length > 1)
            {
                // Gets different track
                int randomIndex;
                do
                {
                    randomIndex = Random.Range(0, clips.Length);
                } while (randomIndex != indexOfTrack);

                // Plays track and stores that index in memory
                source.clip = clips[randomIndex];
                shouldPlay = true;
                indexOfTrack = randomIndex;
            }
            else
            {
                // Loop through tracks linearly
                source.clip = clips[indexOfTrack];
                shouldPlay = true;
                indexOfTrack++;
                if (indexOfTrack >= clips.Length && shouldRepeat)
                    indexOfTrack = 0;
            }
        }

        public void Update()
        {
            // If end of tracks and shouldn't repeat, leave
            if ((!shouldRepeat && indexOfTrack >= clips.Length) || !source || !source.clip)
                return;

            // Play when !source.isPlaying, and !paused if we care about that, and shouldPlay
            // Pause whn source.isPlaying, and paused if we care about that or !shouldPlay
            if (shouldPlay && !source.isPlaying && (playWhilePaused || !PauseMenuManager.IsPaused))
            {
                // Check for end of clip
                if (source.time >= source.clip.length)
                {
                    // End of clip
                    Play(source);
                    return;
                }
                source.Play();
            }
            else if (source.isPlaying && ((!playWhilePaused && PauseMenuManager.IsPaused) || !shouldPlay))
            {
                source.Pause();
            }
        }

    }

}
