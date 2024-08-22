using UnityEngine;
using UnityEngine.Audio;
using SWAssets;
using UnityEngine.Rendering;
using System.Collections.Generic;

public class GameAssets : Singleton<GameAssets> {

	[Header("Prefabs")]
	public Transform MessagePrefab;
	public Transform QTEPrefab;
	public Transform SequencePrefab;
	public Transform MazePrefab;
    public PrefabData<string>[] MazeFiles;

    [Header("Log Entries")]
    public RectTransform LogButton;
    public RectTransform Spacer;
	public List<LogEntry> LogEntries = new();

    [Header("Post Processing")]
    public VolumeProfile VolumeProfile;

    [Header("Sound")]
    public AudioMixer MainMixer;
    public Audio SuccessSoundEffect;
    public Audio FailureSoundEffect;
    public Audio MenuMusicAudio;
    public Audio LevelMusicAudio;

	void Awake()
	{
		RegisterSingleton(this);
    }

    private void Update()
    {
        SuccessSoundEffect.Update();
        FailureSoundEffect.Update();
        MenuMusicAudio.Update();
        LevelMusicAudio.Update();
    }

    public void ChangeMasterVolume(float vol) => MainMixer.SetFloat("Master", vol - 80f);
    public void ChangeSFXVolume(float vol) => MainMixer.SetFloat("SFX", vol - 80f);
    public void ChangeUIVolume(float vol) => MainMixer.SetFloat("UI", vol - 80f);
    public void ChangeMusicVolume(float vol) => MainMixer.SetFloat("Music", vol - 80f);

	[System.Serializable]
    public class PrefabData<T>
    {
        public T obj;
        public float chance;
    }

    [System.Serializable]
    public struct LogEntry
    {
        public string Name;
		[TextArea] public string Contents;
        public Sprite CoverImage;
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
        int indexOfTrack;
        AudioSource source;

        public void Play(AudioSource source)
        {
            if (GameValues.GetActiveBuildIndex() != 0 && playInMainMenu)
                return;

            if (!shouldRepeat && indexOfTrack >= clips.Length)
                return;
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
				source.Play();
                indexOfTrack = randomIndex;
			}
            else
            {
                // Loop through tracks linearly
                source.clip = clips[indexOfTrack];
                source.Play();
                indexOfTrack++;
                if (indexOfTrack >= clips.Length && shouldRepeat)
                    indexOfTrack = 0;
            }
        }

        public void Update()
        {
            // If end of tracks and shouldn't repeat, leave
            if (!shouldRepeat && indexOfTrack >= clips.Length)
                return;

            // Finished playing
            if (source.time >= source.clip.length)
            {
                // Repeat
                Play(source);
            }
        }

    }

    public T GetRandomPrefab<T>(PrefabData<T>[] prefabDatas)
    {
        float totalSpawnChance = 0f;
        foreach (PrefabData<T> enemyPrefabData in prefabDatas)
        {
            totalSpawnChance += enemyPrefabData.chance;
        }

        // Generate a random number between 0 and the total spawn chance
        float randomValue = Random.Range(0f, totalSpawnChance);

        // Find the first gameobject that has the chance
        T selectedEnemyPrefab = default;
        foreach (PrefabData<T> prefabData in prefabDatas)
        {
            if (randomValue < prefabData.chance)
            {
                selectedEnemyPrefab = prefabData.obj;
                break;
            }
            randomValue -= prefabData.chance;
        }

        return selectedEnemyPrefab;
    }

}
