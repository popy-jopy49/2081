using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class PlayVideo : MonoBehaviour
{
    [SerializeField] private VideoPlayer videoPlayer;
    [SerializeField] private bool isIntro;
    private bool _playingCutscene;

    private void Start()
    {
        videoPlayer.loopPointReached += LoopPointReached;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player") || _playingCutscene)
            return;

        _playingCutscene = true;
        videoPlayer.gameObject.SetActive(true);
    }

    private void LoopPointReached(VideoPlayer vp)
    {
        if (isIntro)
        {
            videoPlayer.gameObject.SetActive(false);
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            SceneManager.LoadScene(0);
        }
    }
}
