using SWAssets;
using UnityEngine;

public class GameValues : Singleton<GameValues>
{

    [Header("Grenade")]
    public float TimeToExplosion;
    public int GrenadeCount = 10;

    [Header("Booster")]
    public int SpeedIncrease;

    [Header("Explosives")]
    public float ExplosionForce;
	public float Radius;

	[Header("Missile")]
    public float MissileSpeed;

    [Header("Power Up Times")]
    public float BoosterTime;
    public float SpikesTime;

    [Space(10)]
    public float PowerUpCooldown;
    public float PowerUpSpawnCooldown;

    void Awake()
    {
        RegisterSingleton(this);
    }

}
