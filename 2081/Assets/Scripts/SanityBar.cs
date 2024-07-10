using UnityEngine;
using UnityEngine.UI;

public class SanityBar : MonoBehaviour
{

    private Slider slider;

    private void Awake()
    {
        // Get Slider UI and call function when sanity is changed
        slider = GetComponent<Slider>();
        Player.OnSanityChanged += OnSanityChanged;
    }

    private void OnSanityChanged(object sender, (float current, float max) sanity)
    {
        // Normalise sanity and set UI value
        slider.value = sanity.current / sanity.max;
    }

}
