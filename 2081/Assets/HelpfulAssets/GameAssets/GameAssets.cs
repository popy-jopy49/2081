using UnityEngine;
using UnityEngine.Audio;
using SWAssets;
using UnityEngine.Rendering;
using System.Collections.Generic;

public class GameAssets : Singleton<GameAssets> {

	[Header("Prefabs")]
	public GameObject MessagePrefab;
	public GameObject MinePrefab;
	public GameObject BoosterPrefab;
	public GameObject SpikesPrefab;
	public GameObject MissilePrefab;
	public GameObject GrenadePrefab;
	public GameObject PowerupPrefab;
	public List<Sprite> PowerupSprites;

    [Header("Effects")]
    public ParticleSystem ExplosionPS;

    [Header("Post Processing")]
    public VolumeProfile VolumeProfile;

    [Header("Mixers")]
    public AudioMixer MainMixer;

	void Awake()
	{
		RegisterSingleton(this);
	}

}
