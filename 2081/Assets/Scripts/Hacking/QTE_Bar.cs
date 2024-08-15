using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

public class QTE_Bar : MonoBehaviour
{

	public EventHandler<bool> OnComplete;
	private float totalQTEWidth = 410f;
	private float barWidth;
	private float moveAmount;
	private RectTransform slider;
	private RectTransform bar;
	private bool negativeMovement = false;
	private float barXPos;

	public void Setup(float _moveAmount, float _barWidth)
	{
		moveAmount = _moveAmount;
		barWidth = _barWidth;

		// Setup bar
		bar = transform.Find("Bar").GetComponent<RectTransform>();
		// Set size difference to the remaining amount to get to the desired size
		bar.sizeDelta = new Vector2(barWidth, 0);
		// Random between left extent + width of bar and right extent - width of bar
		barXPos = Random.Range(-totalQTEWidth + barWidth, totalQTEWidth - barWidth);
		bar.anchoredPosition = new Vector2(barXPos, 0);
	}

	private void Awake()
	{
		// Find slider and subscribe to input
		slider = transform.Find("Slider").GetComponent<RectTransform>();
	}

    private void OnEnable()
    {
        InputManager.MAIN.Character.Interact.started += OnPress;
    }

    private void OnDisable()
    {
        InputManager.MAIN.Character.Interact.started -= OnPress;
    }

    private void OnPress(InputAction.CallbackContext obj)
    {
		// If it is not this bar's time, don't call this function
		if (!enabled)
			return;

        // Check if outside zone and return
        if (slider.anchoredPosition.x < barXPos - barWidth/2 || slider.anchoredPosition.x > barXPos + barWidth/2)
		{
			OnComplete?.Invoke(this, false);
			return;
		}

		// Move onto next zone or complete event
		OnComplete?.Invoke(this, true);
		enabled = false;
	}

	private void Update()
	{
		// Make slider move left and right
		slider.anchoredPosition = new Vector2(slider.anchoredPosition.x + (moveAmount * (negativeMovement ? -1 : 1)), 0);

		// Reverse direction if on the edge of movement
		if (slider.anchoredPosition.x <= -totalQTEWidth + slider.rect.width * 2)
			negativeMovement = false;
		else if (slider.anchoredPosition.x >= totalQTEWidth - slider.rect.width * 2)
			negativeMovement = true;
	}

}
