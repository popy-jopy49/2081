using UnityEngine;
using UnityEngine.Audio;
using SWAssets;
using UnityEngine.Rendering;

public class GameAssets : Singleton<GameAssets> {

	[Header("Prefabs")]
	public Transform MessagePrefab;
	public Transform QTEPrefab;
	public Transform SequencePrefab;
	public Transform MazePrefab;
    public PrefabData<string>[] MazeFiles;

    [Header("UI")]
    public RectTransform LogButton;
    public RectTransform Spacer;

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

    [System.Serializable]
    public class PrefabData<T>
    {
        public T obj;
        public float chance;
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

        public void Play(AudioSource source)
        {
            if (GameValues.GetActiveBuildIndex() != 0 && playInMainMenu)
                return;

            if (!shouldRepeat && indexOfTrack >= clips.Length)
                return;

            // In the correct scene
            if (shouldShuffle && clips.Length > 1)
			{
                int randomIndex;
				do
				{
					randomIndex = Random.Range(0, clips.Length);
				} while (randomIndex != indexOfTrack);

				source.clip = clips[randomIndex];
				source.Play();
                indexOfTrack = randomIndex;
			}
            else
            {
                source.clip = clips[indexOfTrack];
                source.Play();
                indexOfTrack++;
                if (indexOfTrack >= clips.Length && shouldRepeat)
                    indexOfTrack = 0;
            }

            
        }

        // Repeat Play after end of track
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
