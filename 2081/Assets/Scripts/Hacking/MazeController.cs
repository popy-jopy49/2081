using UnityEngine;
using UnityEngine.EventSystems;

public class MazeController : MonoBehaviour, IDragHandler
{
	PuzzleGrid grid;
	private (int x, int y) prevIndex = default;
	RectTransform rectTransform;

	// Reference grid in puzzle
	private void Awake()
	{
		rectTransform = GetComponent<RectTransform>();
		grid = transform.parent.parent.Find("Grid").GetComponent<PuzzleGrid>();
	}

	public void OnDrag(PointerEventData eventData)
	{
		// Grab mouse input
		Vector2 mouseOffset = new Vector2(Screen.currentResolution.width, Screen.currentResolution.height) / 2f;
		Vector2 mousePos = (InputManager.MAIN.Character.MousePosition.ReadValue<Vector2>() - mouseOffset) / 2f;

		// Convert to grid pos
		(int x, int y) draggedIndex = grid.CanvasToGridPos(mousePos);

		// Exit if same index
        if (prevIndex == draggedIndex)
			return;

		// Find current player position
		(int x, int y) playerIndex = grid.CanvasToGridPos(rectTransform.localPosition);

        print(grid.AreNeighbours(draggedIndex, playerIndex)); // Returning false
        print(grid.grid[draggedIndex.x, draggedIndex.y].OpenPos());
        // check if valid move position
        if (!grid.AreNeighbours(draggedIndex, playerIndex) || !grid.grid[draggedIndex.x, draggedIndex.y].OpenPos())
			return;

		// Valid move position
		// Switch hasPlayer around
		grid.grid[playerIndex.x, playerIndex.y].hasPlayer = false;
		grid.grid[draggedIndex.x, draggedIndex.y].hasPlayer = true;

		// move to new pos
		rectTransform.localPosition = grid.GridToCanvasPos(draggedIndex, grid.GetGridOffset());

		// Check if the player is now on the winning area
		if (grid.grid[draggedIndex.x, draggedIndex.y].win)
		{
			grid.Win();
		}
	}
}
