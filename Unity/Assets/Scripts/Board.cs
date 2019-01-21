using UnityEngine;
using UnityEngine.UI;
using System.Collections;

// New
public enum CellState
{
    None,
    Friendly,
    Enemy,
    Free,
    OutOfBounds
}

public class Board : MonoBehaviour
{
    public GameObject mCellPrefab;

    [HideInInspector]
    public Cell[,] mAllCells = new Cell[8, 8];

    [HideInInspector]
    public LaserBeam[,,] mAllLaserBeams = new LaserBeam[7, 8, 2];

    public void Create()
    {
        for (int y = 0; y < 8; y++)
        {
            for (int x = 0; x < 8; x++)
            {
                //  Create the cell
                GameObject newCell = Instantiate(mCellPrefab, transform);

                //  Position
                RectTransform rectTransform = newCell.GetComponent<RectTransform>();
                rectTransform.anchoredPosition = new Vector2((x * 100)-350, (y * 100)-350);

                //  Setup
                mAllCells[x, y] = newCell.GetComponent<Cell>();
                mAllCells[x, y].Setup(new Vector2Int(x, y), this);
            }
        }

        for (int x = 0; x < 8; x += 2 )
        {
            for (int y = 0; y < 8; y++)
            {
                //  Offset for every other line
                int offset = (y % 2 != 0) ? 0 : 1;
                int finalX = x + offset;

                //color
                mAllCells[finalX, y].GetComponent<Image>().color = new Color32(230, 220, 187, 255);
            }
        }
    }

    public CellState ValidateCell(int targetX, int targetY, BasePiece checkingPiece)
    {   
        //bounds check
        if(targetX < 0 || targetX > 7)
        {
            return CellState.OutOfBounds;
        }

        if(targetY < 0 || targetY > 7)
        {
            return CellState.OutOfBounds;
        }

        Cell targetCell = mAllCells[targetX, targetY];

        //if the cell has a peice
        if(targetCell.mCurrentPiece != null)
        {
            //if friendly
            if (checkingPiece.mColor == targetCell.mCurrentPiece.mColor)
            {
                return CellState.Friendly;
            }

            if(checkingPiece.mColor != targetCell.mCurrentPiece.mColor)
            {
                return CellState.Enemy;
            }
        }

        return CellState.Free;
    }

    public CellState ValidateLaser(int targetX, int targetY)
    {
        //bounds check
        if (targetX < 0 || targetX > 7)
        {
            return CellState.OutOfBounds;
        }

        if (targetY < 0 || targetY > 7)
        {
            return CellState.OutOfBounds;
        }

        Cell targetCell = mAllCells[targetX, targetY];

        //if the cell has a peice
        if (targetCell.mCurrentPiece != null)
        {
            return CellState.Friendly;
        }

        return CellState.Free;
    }
}
