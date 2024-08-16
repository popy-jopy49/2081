using UnityEngine;
using System;
using System.IO;

public class PuzzleGrid : HackPuzzle
{
    public GridObject[,] grid;
    [SerializeField] private Vector2 gridSize;
    [SerializeField] private Vector2 gridObjectSize;
    private string fileName = "";
    [SerializeField] private TextPrefabDigit[] textPrefabDigits;
    private Vector2 gridOffset;

    //private Transform cameraParent;

    public void Awake()
    {
        fileName = GameAssets.I.GetRandomPrefab(GameAssets.I.MazeFiles);

        //cameraParent = GameValues.GetCamera().transform;
        string text = GetTextAtPath(fileName);
        InitialiseGrid(text);
    }

    private void InitialiseGrid(string text)
    {
        // set grid offset and dimensions
        gridOffset = (gridSize * gridObjectSize - gridObjectSize) / 2f;
        gridOffset.x *= -1;
        grid = new GridObject[(int)gridSize.x, (int)gridSize.y];

        // Loop through every row and column
        int dataIndex = 0;
        for (int y = 0; y < grid.GetLength(1); y++)
        {
            for (int x = 0; x < grid.GetLength(0); x++)
            {
                // Set up grid objects and each position
                char digit = text[dataIndex];
                Vector2 pos = GridToCanvasPos((x, y), gridOffset);
                grid[x, y] = new GridObject(digit, pos, gridObjectSize, transform, textPrefabDigits);
                SWAssets.Utils.UIUtils.DrawUIText(x + ", " + y, transform, pos, 18, null);
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

    public bool AreNeighbours((int x, int y) firstIndex, (int x, int y) secondIndex)
    {
        // Checks they are both grid positions and are different
        if (!IsValidGridPosition(firstIndex) || !IsValidGridPosition(secondIndex) || firstIndex == secondIndex)
            return false;
        print("First: " + firstIndex + " Second: " + secondIndex);
        //print("X: " + (Mathf.Abs(firstIndex.x - secondIndex.x) == 1));
        //print("Y: " + (Mathf.Abs(firstIndex.y - secondIndex.y) == 1));
        //print("XY: " + (firstIndex.y == secondIndex.y));
        //print("YX: " + (firstIndex.x == secondIndex.x));
        // checks directly left, right, up, and down
        return (Mathf.Abs(firstIndex.x - secondIndex.x) == 1 && firstIndex.y == secondIndex.y) ^
            (Mathf.Abs(firstIndex.y - secondIndex.y) == 1 && firstIndex.x == secondIndex.x);
    }

    public (int x, int y) CanvasToGridPos(Vector2 pos)
    {
        // Reverses grid to canvas transformations
        Vector2 index = pos;
        index -= gridOffset;
        index /= gridObjectSize;
        index.y *= -1;
        return (Mathf.RoundToInt(index.x), Mathf.RoundToInt(index.y));
    }

    // applies reverse transformations to those in the setup
    public Vector2 GridToCanvasPos((int x, int y) index, Vector2 gridOffset)
    {
        return new Vector2(index.x, -index.y) * gridObjectSize + gridOffset;
    }

    public bool IsValidGridPosition((int x, int y) index)
    {
        // Has to be in the grid
        return index.x >= 0 && index.x < grid.GetLength(0) &&
               index.y >= 0 && index.y < grid.GetLength(1);
	}

	public void Win()
	{
        OnPuzzleComplete?.Invoke(this, true);
	}

	public Vector2 GetGridOffset() => gridOffset;

    public class GridObject
    {
        public bool hasPlayer = false;
        readonly bool wall = false;
        public bool win = false;
        //char digit;

        // Setup grid position
        public GridObject(char digit, Vector2 pos, Vector2 size, Transform parent, TextPrefabDigit[] textPrefabDigits)
        {
            //this.digit = digit;
            Transform prefab = null;
            // Setup varaibles from struct
            foreach (TextPrefabDigit tPD in textPrefabDigits)
            {
                if (digit != tPD.digit)
                    continue;

                hasPlayer = tPD.player;
                wall = tPD.wall;
                win = tPD.win;
                prefab = tPD.prefab;
                break;
            }
            if (!prefab)
                return;

            RectTransform obj = Instantiate(prefab, parent).GetComponent<RectTransform>();
            obj.anchoredPosition = pos;
            obj.sizeDelta = size;

            // Will change
            //if (obj.TryGetComponent(out PuzzleWin win)) win.SetWinFunc(obj.parent.GetComponent<PuzzleGrid>());
        }

        public bool OpenPos() => !wall;
        //public char GetDigit() => digit;
        //public void SetDigit(char digit) => this.digit = digit;
    }

    [Serializable]
    public struct TextPrefabDigit
    {
        public char digit;
        public Transform prefab;
        public bool wall;
        public bool player;
        public bool win;
    }

}
