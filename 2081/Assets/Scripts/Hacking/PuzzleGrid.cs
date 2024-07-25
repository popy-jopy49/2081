using UnityEngine;
using System;
using System.IO;
using System.Collections.Generic;

public class PuzzleGrid : MonoBehaviour
{
    public GridObject[,] grid;
    [SerializeField] private Vector2 gridSize;
    [SerializeField] private Vector2 gridObjectSize;
    [SerializeField] private string fileName = "";
    [SerializeField] private string comparisonFileName = "";
    [SerializeField] private Transform parent;
    [SerializeField] private TextPrefabDigit[] textPrefabDigits;
    private Vector2 gridOffset;

    private Action winFunc;
    private Transform cameraParent;

    // Global static function to instantiate new grid
    /*public static PuzzleGrid Setup(Transform prefab, GameAssets.PrefabData<string>[] files, Action winFunc, GameAssets.PrefabData<string>[] comparisonFiles = null)
	{
		PuzzleGrid grid = Instantiate(prefab, Camera.main.transform.Find("Puzzles")).Find("Grid").GetComponent<PuzzleGrid>();
        grid.fileName = GameAssets.I.GetRandomPrefab(files, out int index);

        grid.cameraParent = Camera.main.transform;
		grid.SetWinFunc(winFunc);
        string text = grid.GetTextAtPath(grid.fileName);
        grid.InitialiseGrid(text);
        return grid;
    }*/

    private void InitialiseGrid(string text)
	{
        // set grid offset and dimensions
        gridOffset = (gridSize * gridObjectSize - gridObjectSize) / 2f;
		gridOffset.x *= -1;
		grid = new GridObject[(int)gridSize.x, (int)gridSize.y];
        Vector3 scale = gridSize / 2f;
        scale.z = 1;
		transform.parent.Find("Background").localScale = scale;

        // Loop through every row and column
		int dataIndex = 0;
		for (int y = 0; y < grid.GetLength(1); y++)
		{
			for (int x = 0; x < grid.GetLength(0); x++)
			{
                // Set up grid objects and each position
				char digit = text[dataIndex];
				Vector2 pos = GridToWorldPos((x, y));
				grid[x, y] = new GridObject(digit, pos, gridObjectSize, parent, textPrefabDigits, winFunc);
				dataIndex++;
			}
		}
	}

    private string GetTextAtPath(string path)
	{
        // reads file at path
		StreamReader reader = new($"Assets/Resources/{path}");
		string text = reader.ReadToEnd().Trim().Replace("\n", "").Replace("\r", "");
		reader.Close();
		return text;
    }

    public bool ValidMovePosition((int x, int y) index)
    {
        if (!IsValidGridPosition(index))
            return false;

        // Check if this grid tile is open
        bool free = grid[index.x, index.y].OpenPos();
        bool neighboursHavePlayer = false;

		List<(int x, int y)> neighbours = new()
        {
            (index.x + 1, index.y),
            (index.x - 1, index.y),
            (index.x, index.y + 1),
            (index.x, index.y - 1),
        };
        // Loop through neibours to see if they have the player
        foreach ((int x, int y) neighbour in neighbours)
		{
			if (!IsValidGridPosition(neighbour))
				continue;

			if (grid[neighbour.x, neighbour.y].hasPlayer)
			{
				neighboursHavePlayer = true;
				break;
			}
		}

		return free && neighboursHavePlayer;
	}

    public bool AreNeighbours((int x, int y) firstIndex, (int x, int y) secondIndex)
    {
        // Checks they are both grid positions and are different
        if (!IsValidGridPosition(firstIndex) || !IsValidGridPosition(secondIndex) || firstIndex == secondIndex)
            return false;

        // checks directly left, right, up, and down
        return (Mathf.Abs(firstIndex.x - secondIndex.x) == 1 && firstIndex.y == secondIndex.y) ^
            (Mathf.Abs(firstIndex.y - secondIndex.y) == 1 && firstIndex.x == secondIndex.x);
    }

	public (int x, int y) WorldToGridPos(Vector2 pos)
	{
        // Reverses grid to world transformations
		Vector2 index = pos;
		index -= gridOffset;
        index -= (Vector2)cameraParent.position;
		index /= gridObjectSize;
		index.y *= -1;
		return (Mathf.RoundToInt(index.x), Mathf.RoundToInt(index.y));
	}

    // applies reverse transformations to those in the setup
	public Vector2 GridToWorldPos((int x, int y) index)
	{
		return new Vector2(index.x, -index.y) * gridObjectSize + gridOffset + (Vector2)cameraParent.position;
	}

    // compares grid digits to those of a file
    public bool CompareGrid()
    {
        string text = GetTextAtPath(comparisonFileName);
        int index = 0;
        for (int y = 0; y < grid.GetLength(1); y++)
        {
            for (int x = 0; x < grid.GetLength(0); x++)
            {
                if (index >= text.Length || grid[x, y].GetDigit() != text[index])
                    return false;

                index++;
            }
        }

        return true;
    }

	public bool IsValidGridPosition((int x, int y) index)
    {
        // Has to be in the grid
        return index.x >= 0 && index.x < grid.GetLength(0) &&
			   index.y >= 0 && index.y < grid.GetLength(1);
	}

    public void SetWinFunc(Action func) => winFunc = func;

	public class GridObject
    {
        public bool hasPlayer = false;
		readonly bool wall = false;
		char digit;

        // Setup grid position
        public GridObject(char digit, Vector2 pos, Vector2 size, Transform parent, TextPrefabDigit[] textPrefabDigits, Action winFunc)
        {
            this.digit = digit;
            Transform prefab = null;
            // Setup varaibles from struct
            foreach (TextPrefabDigit tPD in textPrefabDigits)
            {
                if (digit != tPD.digit)
                    continue;

				hasPlayer = tPD.player;
                wall = tPD.wall;
                prefab = tPD.prefab;
                break;
            }
            if (!prefab)
                return;

            Transform obj = Instantiate(prefab, parent);
            obj.position = pos;
			obj.localScale = size;

			PuzzleWin win = obj.GetComponent<PuzzleWin>();
			if (win) win.SetWinFunc(winFunc);
		}

        public bool OpenPos() => !wall;
        public char GetDigit() => digit;
        public void SetDigit(char digit) => this.digit = digit;
	}

    [Serializable]
    public struct TextPrefabDigit
    {
        public char digit;
        public Transform prefab;
        public bool wall;
        public bool player;
    }

}
