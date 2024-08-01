using SWAssets;
using UnityEngine;
using static Michsky.UI.Heat.PauseMenuManager;

public class GameValues : Singleton<GameValues>
{

    [Header("QTE")]
    public float minMoveAmount;
	public float maxMoveAmount;
	public float minBarWidth;
	public float maxBarWidth;

    [Header("Sequence")]
    public int minNumberOfSequences;
    public int maxNumberOfSequences;

    //[Header("Global Variables")]
    private static Camera CAMERA;
    private static Transform CANVAS_HUD;
    [HideInInspector] public static bool IN_PUZZLE = false;
    public CursorLockMode MenuCursorState = CursorLockMode.None;
    public CursorLockMode GameCursorState = CursorLockMode.Locked;
    public CursorVisibility MenuCursorVisibility = CursorVisibility.Visible;
    public CursorVisibility GameCursorVisibility = CursorVisibility.Invisible;

    public enum CursorVisibility { Default, Invisible, Visible }

    void Awake()
    {
        RegisterSingleton(this);
        CAMERA = GameObject.Find("Player").transform.Find("Camera").GetComponent<Camera>();
        CANVAS_HUD = GameObject.Find("Canvas - HUD").transform;
    }

    public static Camera GetCamera() => CAMERA;
    public static Transform GetCanvas() => CANVAS_HUD;

}
