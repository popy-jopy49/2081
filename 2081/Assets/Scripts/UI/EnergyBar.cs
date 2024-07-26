using Michsky.UI.Heat;
using UnityEngine;

public class EnergyBar : MonoBehaviour
{

    private ProgressBar slider;

    private void Awake()
	{
		// Get Slider UI and call function when energy is changed
		slider = GetComponent<ProgressBar>();
        CharacterControllerFPS.OnEnergyChanged += OnEnergyChanged;
    }

    private void OnEnergyChanged(object sender, (float current, float max) energy)
	{
		// Set UI values
		slider.maxValueLimit = energy.max;
		slider.maxValue = energy.max;
		slider.currentValue = energy.current;
    }

}
