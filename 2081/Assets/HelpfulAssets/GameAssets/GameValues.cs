using SWAssets;
using UnityEngine;

public class GameValues : Singleton<GameValues>
{

    [Header("QTE")]
    public float minMoveAmount;
	public float maxMoveAmount;
	public float minBarWidth;
	public float maxBarWidth;

    //[Header("Global Variables")]
    private static Camera CAMERA;
    private static Transform CANVAS;

    void Awake()
    {
        RegisterSingleton(this);
        CAMERA = GameObject.Find("Player").transform.Find("Camera").GetComponent<Camera>();
        CANVAS = GameObject.Find("Canvas").transform;
    }

    public static Camera GetCamera() => CAMERA;
    public static Transform GetCanvas() => CANVAS;

}
