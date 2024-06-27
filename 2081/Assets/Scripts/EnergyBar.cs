using UnityEngine;
using UnityEngine.UI;

public class EnergyBar : MonoBehaviour
{

    private Slider slider;

    private void Awake()
    {
        slider = GetComponent<Slider>();
        CharacterControllerFPS.OnEnergyChanged += OnEnergyChanged;
    }

    private void OnEnergyChanged(object sender, (float current, float max) e)
    {
        slider.value = e.current / e.max;
    }

}
