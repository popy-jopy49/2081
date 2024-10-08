using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace Michsky.UI.Heat
{
    public class PauseMenuManager : MonoBehaviour
    {
        // Resources
        public GameObject pauseMenuCanvas;
        [SerializeField] private ButtonManager continueButton;
        [SerializeField] private PanelManager panelManager;
        [SerializeField] private ImageFading background;

        // Settings
        [SerializeField] private bool setTimeScale = true;
        [Range(0, 1)] public float inputBlockDuration = 0.2f;
        [SerializeField] private InputAction hotkey;

        // Events
        public UnityEvent onOpen = new UnityEvent();
        public UnityEvent onClose = new UnityEvent();

        // Helpers
        public static bool IsPaused = false;
        bool allowClosing = true;
        float disableAfter = 0.6f;

        void Awake()
        {
            if (pauseMenuCanvas == null)
            {
                Debug.LogError("<b>[Pause Menu Manager]</b> Pause Menu Canvas is missing!", this);
                this.enabled = false;
                return;
            }

            pauseMenuCanvas.SetActive(true);
        }

        void Start()
        {
            if (panelManager != null) { disableAfter = HeatUIInternalTools.GetAnimatorClipLength(panelManager.panels[panelManager.currentPanelIndex].panelObject, "MainPanel_Out"); }
            if (continueButton != null) { continueButton.onClick.AddListener(ClosePauseMenu); }

            pauseMenuCanvas.SetActive(false);
            hotkey.Enable();
        }

        void Update()
        {
            if (hotkey.triggered) { AnimatePauseMenu(); }
        }

        public void AnimatePauseMenu()
        {
            if (!IsPaused) { OpenPauseMenu(); }
            else { ClosePauseMenu(); }
        }

        public void OpenPauseMenu()
        {
            if (IsPaused) { return; }
            if (setTimeScale) { Time.timeScale = 0; }
            if (inputBlockDuration > 0)
            {
                AllowClosing(false);
                StopCoroutine("InputBlockProcess");
                StartCoroutine("InputBlockProcess");
            }

            StopCoroutine("DisablePauseCanvas");

            IsPaused = true;
            onOpen.Invoke();

            pauseMenuCanvas.SetActive(false);
            pauseMenuCanvas.SetActive(true);

            FadeInBackground();

            if (continueButton != null && Gamepad.current != null)
            {
                EventSystem.current.SetSelectedGameObject(null);
                EventSystem.current.SetSelectedGameObject(continueButton.gameObject);
            }

            Cursor.lockState = GameValues.I.MenuCursorState;

            if (GameValues.I.MenuCursorVisibility == GameValues.CursorVisibility.Visible) { Cursor.visible = true; }
            else if (GameValues.I.MenuCursorVisibility != GameValues.CursorVisibility.Default) { Cursor.visible = false; }
        }

        public void ClosePauseMenu()
        {
            if (!IsPaused || !allowClosing) { return; }
            if (setTimeScale == true) { Time.timeScale = 1; }
            if (panelManager != null) { panelManager.HideCurrentPanel(); }

            StopCoroutine("DisablePauseCanvas");
            StartCoroutine("DisablePauseCanvas");


            IsPaused = false;
            onClose.Invoke();

            FadeOutBackground();

            // If In puzzle, don't go game
            if (GameValues.IN_PUZZLE)
                return;

            if (GameValues.I.GameCursorVisibility == GameValues.CursorVisibility.Visible) { Cursor.visible = true; }
            else if (GameValues.I.GameCursorVisibility != GameValues.CursorVisibility.Default) { Cursor.visible = false; }

            Cursor.lockState = GameValues.I.GameCursorState;
        }

        public void FadeInBackground()
        {
            if (background == null)
                return;

            background.FadeIn();
        }

        public void FadeOutBackground()
        {
            if (background == null)
                return;

            background.FadeOut();
        }

        public void AllowClosing(bool value)
        {
            allowClosing = value;
        }

        IEnumerator DisablePauseCanvas()
        {
            yield return new WaitForSecondsRealtime(disableAfter);
            pauseMenuCanvas.SetActive(false);
        }

        IEnumerator InputBlockProcess()
        {
            yield return new WaitForSecondsRealtime(inputBlockDuration);
            AllowClosing(true);
        }
    }
}