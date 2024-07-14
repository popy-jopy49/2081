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
		bar.sizeDelta = new Vector2(barWidth - bar.rect.width, 0);
		// Random between left extent + width of bar and right extent - width of bar
		barXPos = Random.Range(-totalQTEWidth + barWidth, totalQTEWidth - barWidth);
		bar.localPosition = new Vector2(barXPos, 0);
	}

	private void Awake()
	{
		// Find slider and subscribe to input
		slider = transform.Find("Slider").GetComponent<RectTransform>();
		InputManager.MAIN.Character.Interact.started += OnPress;
	}

	private void OnPress(InputAction.CallbackContext obj)
	{
		// Check if outside zone and return
		if (slider.localPosition.x < barXPos - barWidth || slider.localPosition.x > barXPos + barWidth)
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
		slider.localPosition = new Vector2(slider.localPosition.x + (moveAmount * (negativeMovement ? -1 : 1)), 0);

		// Reverse direction if on the edge of movement
		if (slider.localPosition.x <= -totalQTEWidth + slider.rect.width * 2)
			negativeMovement = false;
		else if (slider.localPosition.x >= totalQTEWidth - slider.rect.width * 2)
			negativeMovement = true;
	}

}
