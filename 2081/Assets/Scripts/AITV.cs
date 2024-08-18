using Michsky.UI.Heat;
using UnityEngine;

public class AITV : MonoBehaviour
{

	[SerializeField] private float updateStep = 0.1f; // Update step in seconds
	[SerializeField] private int sampleDataLength = 1024;
	[SerializeField] private AnimationCurve curve;

	private AudioSource source;

	private float[] clipSampleData;
	private RectTransform[] loudnessIndicators;
	private bool shouldPlay = false;

	private void Awake()
	{
		source = GetComponent<AudioSource>();
		clipSampleData = new float[sampleDataLength];
		loudnessIndicators = transform.Find("Screen").Find("Contents").Find("Audio").GetComponentsInChildren<RectTransform>();
	}

	private void OnTriggerEnter(Collider other)
	{
		// CHeck if it is the player
		if (!other.CompareTag("Player"))
			return;

		// Begin playing if there is a clip
		if (!source.clip)
		{
			Debug.LogError($"{GetType()}.OnTriggerEnter: No clip assigned to audio source.");
			return;
		}
		shouldPlay = true;
		InvokeRepeating(nameof(SetLoudness), updateStep, updateStep);
	}

	private void OnTriggerExit(Collider other)
	{
		// CHeck if it is the player
		if (!other.CompareTag("Player"))
			return;

		// Begin playing if there is a clip
		if (!source.clip)
		{
			Debug.LogError($"{GetType()}.OnTriggerEnter: No clip assigned to audio source.");
			return;
		}
		shouldPlay = false;
		CancelInvoke();
	}

	private void Update()
	{
		// Play when ! sour.isPlaying and !paused and shouldPlay
		// Pause whn source.isPlaying and paused or !shouldPlay
		if (shouldPlay && !source.isPlaying && !PauseMenuManager.IsPaused)
		{
			source.Play();
		}
		else if (source.isPlaying && (PauseMenuManager.IsPaused || !shouldPlay))
		{
			source.Pause();
		}
	}

	private void SetLoudness()
	{
		// Find clip loudness
		float clipLoudness = FindLoudness();
        for (int i = 0; i < loudnessIndicators.Length; i++)
        {
			float pointAlongCurve = i / (float)loudnessIndicators.Length;
			float distanceFromEdge = curve.Evaluate(pointAlongCurve) * clipLoudness;
			loudnessIndicators[i].sizeDelta = new Vector2(0.001f, distanceFromEdge * 0.005f);
        }
    }

	private float FindLoudness()
	{
		source.clip.GetData(clipSampleData, source.timeSamples); // Read 1024 samples, which is about 80 ms on a 44khz stereo clip, beginning at the current sample position of the clip.
		float totalClipLoudness = 0f;
		foreach (float sample in clipSampleData)
		{
			totalClipLoudness += Mathf.Abs(sample);
		}
		return totalClipLoudness / sampleDataLength; // clipLoudness is an average of all the samples in this time frame
	}

}
