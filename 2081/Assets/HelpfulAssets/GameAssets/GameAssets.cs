using UnityEngine;
using UnityEngine.Audio;
using SWAssets;
using UnityEngine.Rendering;
using System.Collections.Generic;
using Michsky.UI.Heat;
using static SoundManager;

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
    public struct LogEntry
    {
        public string Name;
		[TextArea] public string Contents;
        public Sprite CoverImage;
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
