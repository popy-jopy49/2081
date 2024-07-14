using Random = UnityEngine.Random;

public class QTE : HackPuzzle
{

	QTE_Bar topBar;
	QTE_Bar middleBar;
	QTE_Bar bottomBar;

	private void Awake()
	{
		// Find each bar and set the correct values
		topBar = transform.Find("TopBar").GetComponent<QTE_Bar>();
		topBar.Setup(Random.Range(GameValues.I.minMoveAmount, GameValues.I.maxMoveAmount), 
			Random.Range(GameValues.I.minBarWidth, GameValues.I.maxBarWidth));
		topBar.OnComplete += OnTopComplete;

		middleBar = transform.Find("MiddleBar").GetComponent<QTE_Bar>();
		middleBar.Setup(Random.Range(GameValues.I.minMoveAmount, GameValues.I.maxMoveAmount),
			Random.Range(GameValues.I.minBarWidth, GameValues.I.maxBarWidth));
		middleBar.enabled = false;

		bottomBar = transform.Find("BottomBar").GetComponent<QTE_Bar>();
		bottomBar.Setup(Random.Range(GameValues.I.minMoveAmount, GameValues.I.maxMoveAmount),
			Random.Range(GameValues.I.minBarWidth, GameValues.I.maxBarWidth));
		bottomBar.enabled = false;
	}

	private void OnTopComplete(object sender, bool success)
	{
        if (!success)
        {
			OnPuzzleComplete?.Invoke(this, false);
			return;
        }
        // Unsubscribe previous one and enable and subscribe to new one
        middleBar.enabled = true;
		topBar.OnComplete -= OnTopComplete;
		middleBar.OnComplete += OnMiddleComplete;
	}

	private void OnMiddleComplete(object sender, bool success)
	{
		if (!success)
		{
			OnPuzzleComplete?.Invoke(this, false);
			return;
		}
		// Unsubscribe previous one and enable and subscribe to new one
		middleBar.OnComplete -= OnMiddleComplete;
		bottomBar.enabled = true;
		bottomBar.OnComplete += OnBottomComplete;
	}

	private void OnBottomComplete(object sender, bool success)
	{
		if (!success)
		{
			OnPuzzleComplete?.Invoke(this, false);
			return;
		}
		// Unsubscribe bottom one and complete hack
		bottomBar.OnComplete -= OnBottomComplete;
		OnPuzzleComplete?.Invoke(this, true);
	}

}
