using UnityEngine;
using UnityEngine.Audio;
using SWAssets;
using UnityEngine.Rendering;

public class GameAssets : Singleton<GameAssets> {

	[Header("Prefabs")]
	public GameObject MessagePrefab;
	public GameObject QTEPrefab;
	public GameObject SequencePrefab;
	public GameObject MazeOrSudokuPrefab;

    [Header("Post Processing")]
    public VolumeProfile VolumeProfile;

    [Header("Mixers")]
    public AudioMixer MainMixer;

	void Awake()
	{
		RegisterSingleton(this);
	}

}
