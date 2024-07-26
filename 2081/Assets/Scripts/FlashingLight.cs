using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class FlashingLight : MonoBehaviour
{

    [SerializeField] AnimationCurve curve;
    Light[] lightComponents;

    private void Awake()
    {
        lightComponents = GetComponentsInChildren<Light>();
        StartCoroutine(Flicker());
    }

    private IEnumerator Flicker()
    {
        while (true)
        {
            Array.ForEach(lightComponents, light => light.enabled = true );
            yield return new WaitForSeconds(curve.Evaluate(Random.Range(0f, 1f)));
            Array.ForEach(lightComponents, light => light.enabled = false);
			yield return new WaitForSeconds(curve.Evaluate(Random.Range(0f, 1f)));
		}
    }

}
