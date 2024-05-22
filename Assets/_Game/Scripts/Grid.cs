using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum gridColor
{
    Nothing = 0,
    Blue = 1,
    Green = 2,
    Orange = 3,
    Pink = 4,
    Red = 5,
    Yellow = 6
}

public class Grid : MonoBehaviour
{
    public static bool isOnMatrix;
    public int colums;
    public int rows;
    public float squareGap = 0.1f;
    public float squareScale = 0.5f;

    public GridSquare gridSquareGO;
    public GameObject coin;
    public Vector2 startPosition = new Vector2();
    private Vector2 _offset = Vector2.zero;
    private List<GridSquare> _gridSquares = new List<GridSquare>();

    private void Start()
    {
        EventsInGame.PlaceGrid -= CheckPositionOnBoard;
        EventsInGame.PlaceGrid += CheckPositionOnBoard;
        CreateGrid();
    }

    /// <summary>
    /// Check that the jewels's on matrix or not, or on other jewels
    /// </summary>
    /// <param name="sprite"></param>
    /// <param name="jewels"></param>
    /// <param name="colorName"></param>
    private void CheckPositionOnBoard(Sprite sprite, JewelsController jewels, string colorName)
    {
        isOnMatrix = false;
        for (int i = 0; i < _gridSquares.Count; i++)
        {
            var grid = _gridSquares[i].GetComponent<GridSquare>();
            if (!grid.IsSelected)
            {
                if (grid.CanUseThisGrid())
                {
                    isOnMatrix = true;
                    grid.ActiveGrid(sprite, jewels, colorName);
                    StartCoroutine(IEFindNeighBors(grid));
                }
            }
        }
    }

    private IEnumerator IEFindNeighBors(GridSquare grid)
    {
        yield return new WaitForSeconds(0.2f);
        CheckJewelsTogether(FindNeighbors(grid), grid);
    }

    /// <summary>
    /// Find Neighbors of current grid (not in diagonal line)
    /// </summary>
    /// <param name="targetObject"></param>
    /// <returns></returns>
    public List<GridSquare> FindNeighbors(GridSquare targetObject)
    {
        List<GridSquare> neighbors = new List<GridSquare>();
        int targetIndex = _gridSquares.IndexOf(targetObject);
        if (targetIndex == -1)
        {
            return null; // khong ton tai trong ma tran
        }
        int row = targetIndex / rows;
        int col = targetIndex % colums;

        if (row > 0)
        {
            neighbors.Add(_gridSquares[targetIndex - 6]); // top
        }
        if (row < 5)
        {
            neighbors.Add(_gridSquares[targetIndex + 6]); // below
        }
        if (col > 0)
        {
            neighbors.Add(_gridSquares[targetIndex - 1]); //left
        }
        if (col < 5)
        {
            neighbors.Add(_gridSquares[targetIndex + 1]); // right
        }
        return neighbors;
    }

    /// <summary>
    /// Check jewels is together and deactive them
    /// </summary>
    /// <param name="neighbors"></param>
    /// <param name="grid"></param>
    public void CheckJewelsTogether(List<GridSquare> neighbors, GridSquare grid)
    {
        List<GridSquare> gridsNeighbor = new List<GridSquare>();
        for (int i = 0; i < neighbors.Count; i++)
        {
            if (neighbors[i].gridColor == grid.gridColor)
            {
                gridsNeighbor.Add(neighbors[i]);
            }
        }

        if (gridsNeighbor.Count > 0)
        {
            for (int i = 0; i < gridsNeighbor.Count; i++)
            {
                Vector2 pos = (grid.transform.position + gridsNeighbor[i].transform.position) / 2;
                // should pool instead of Instantiate method
                GameObject newCoin = Instantiate(coin);
                newCoin.transform.position = pos;
                gridsNeighbor[i].DeActiveGrid();
            }
            grid.DeActiveGrid();
        }
    }

    private void CreateGrid()
    {
        SpawnMatrix();
        SetPositionGrid();
    }

    /// <summary>
    /// Spawn matrix 6x6
    /// </summary>
    private void SpawnMatrix()
    {
        int squareIndex = 0;
        for (var i = 0; i < rows; i++)
        {
            for (var j = 0; j < colums; j++)
            {
                _gridSquares.Add(Instantiate(gridSquareGO));
                _gridSquares[_gridSquares.Count - 1].transform.SetParent(this.transform);
                _gridSquares[_gridSquares.Count - 1].transform.localScale = new Vector3(squareScale, squareScale, squareScale);
                if (i % 2 == 0)
                {
                    _gridSquares[_gridSquares.Count - 1].SetImage(squareIndex % 2 == 0);
                }
                else
                {
                    _gridSquares[_gridSquares.Count - 1].SetImage(squareIndex % 2 == 1);
                }
                squareIndex++;
            }
        }
    }

    /// <summary>
    /// Set position of grid on matrix
    /// </summary>
    private void SetPositionGrid()
    {
        int columnNumber = 0;
        int rowNumber = 0;
        Vector2 square_Gap_Number = new Vector2(0.0f, 0.0f);
        bool moveNextRow = false;

        RectTransform square_rect = _gridSquares[0].GetComponent<RectTransform>();

        _offset.x = square_rect.rect.width;
        _offset.y = square_rect.rect.height;

        foreach (GridSquare square in _gridSquares)
        {
            if (columnNumber + 1 > colums)
            {
                square_Gap_Number.x = 0;
                columnNumber = 0;
                rowNumber++;
                moveNextRow = false;
            }

            var pos_x_offset = _offset.x * columnNumber + (square_Gap_Number.x * squareGap);
            var pos_y_offset = _offset.y * rowNumber + (square_Gap_Number.y * squareGap);

            if (columnNumber > 0 && columnNumber % 3 == 0)
            {
                square_Gap_Number.x++;
                pos_x_offset += squareGap;
            }

            if (rowNumber > 0 && rowNumber % 3 == 0 && moveNextRow == false)
            {
                moveNextRow = true;
                square_Gap_Number.y++;
                pos_y_offset += squareGap;
            }
            square.GetComponent<RectTransform>().anchoredPosition = new Vector2(startPosition.x + pos_x_offset, startPosition.y - pos_y_offset);
            square.GetComponent<RectTransform>().localPosition = new Vector3(startPosition.x + pos_x_offset, startPosition.y - pos_y_offset, 0.0f);
            columnNumber++;
        }
    }
}