using SWAssets;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    private static Camera CAMERA;
    private static Transform CANVAS_HUD;
    [HideInInspector] public static bool IN_PUZZLE = false;

    [Header("Mouse Lock Mode")]
    public CursorLockMode MenuCursorState = CursorLockMode.None;
    public CursorLockMode GameCursorState = CursorLockMode.Locked;
    public CursorVisibility MenuCursorVisibility = CursorVisibility.Visible;
    public CursorVisibility GameCursorVisibility = CursorVisibility.Invisible;

    public static float MOUSE_SENSITIVITY = 2.0f;

    public enum SceneIndexes
    {
        MainMenuScene = 0,
		Level1 = 1,
		Level2 = 2,
		Level3 = 3,
		Level4 = 4,
		Level5 = 5,
	}

    public enum CursorVisibility { Default, Invisible, Visible }

    void Awake()
    {
        RegisterSingleton(this);
        CAMERA = GameObject.Find("Player").transform.Find("Camera").GetComponent<Camera>();
        CANVAS_HUD = GameObject.Find("Canvas - HUD").transform;
    }

    public static Camera GetCamera() => CAMERA;
    public static Transform GetCanvas() => CANVAS_HUD;
    public static int GetActiveBuildIndex() => SceneManager.GetActiveScene().buildIndex;
    public void ChangeMouseSensitivity(float sens) => MOUSE_SENSITIVITY = sens;

}
