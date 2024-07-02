using UnityEngine;
using UnityEngine.UI;

public class SanityBar : MonoBehaviour
{

    private Slider slider;

    private void Awake()
    {
        slider = GetComponent<Slider>();
        Player.OnSanityChanged += OnSanityChanged;
    }

    private void OnSanityChanged(object sender, (float current, float max) sanity)
    {
        slider.value = sanity.current / sanity.max;
    }

}
