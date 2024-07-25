using System;
using System.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

public class FlashingLight : MonoBehaviour
{

    [SerializeField] AnimationCurve curve;
    Light[] lightComponents;

    private void Awake()
    {
        lightComponents = GetComponentsInChildren<Light>();
        Flicker();
    }

    private async void Flicker()
    {
        while (true)
        {
            Array.ForEach(lightComponents, light => light.enabled = true );
            await Task.Delay((int)(1000 * curve.Evaluate(Random.Range(0f, 1f))));
            Array.ForEach(lightComponents, light => light.enabled = false);
            await Task.Delay((int)(1000 * curve.Evaluate(Random.Range(0f, 1f))));
        }
    }

}
