using Michsky.UI.Heat;
using UnityEngine;

public class SanityBar : MonoBehaviour
{

    private ProgressBar slider;

    private void Awake()
    {
        // Get Slider UI and call function when sanity is changed
        slider = GetComponent<ProgressBar>();
        Player.OnSanityChanged += OnSanityChanged;
    }

    private void OnSanityChanged(object sender, (float current, float max) sanity)
	{
        // Set UI values
        print(sanity.max);
		slider.maxValueLimit = sanity.max;
		slider.maxValue = sanity.max;
		slider.currentValue = sanity.current;
        slider.UpdateUI();
    }

}
