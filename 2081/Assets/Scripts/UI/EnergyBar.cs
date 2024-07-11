using UnityEngine;
using UnityEngine.UI;

public class EnergyBar : MonoBehaviour
{

    private Slider slider;

    private void Awake()
	{
		// Get Slider UI and call function when energy is changed
		slider = GetComponent<Slider>();
        CharacterControllerFPS.OnEnergyChanged += OnEnergyChanged;
    }

    private void OnEnergyChanged(object sender, (float current, float max) energy)
	{
		// Normalise energy and set UI value
		slider.value = energy.current / energy.max;
    }

}
