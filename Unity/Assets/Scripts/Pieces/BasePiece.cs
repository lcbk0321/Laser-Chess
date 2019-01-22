using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;


public abstract class BasePiece : EventTrigger
{
    [HideInInspector]
    public bool click = false;
    public Color mColor = Color.clear;
    public bool mIsFirstMove = true;

    public int direction = 0;
    public string type = null;

    protected Cell mOriginalCell = null;
    public RectTransform mOriginalTransform = null;
    public Cell mCurrentCell = null;

    public RectTransform mRectTransform = null;
    public PieceManager mPieceManager;

    protected Cell mTargetCell = null;

    protected Vector3Int mMovement = Vector3Int.one;
    public List<Cell> mHighlightedCells = new List<Cell>();

    public virtual void Setup(Color newTeamColor, PieceManager newPieceManager)
    {
        mPieceManager = newPieceManager;
        mColor = newTeamColor;
        mRectTransform = GetComponent<RectTransform>();
    }

    public void Place(Cell newCell)
    {
        //Cell Stuff
        mCurrentCell = newCell;
        mOriginalCell = newCell;
        mCurrentCell.mCurrentPiece = this;

        //Object stuff
        transform.position = newCell.transform.position;
        Debug.Log("Place function in BasePiece: " + transform.position);
        
        Debug.Log(transform.position.x+", "+ transform.position.y);

        gameObject.SetActive(true);
    }

    public void Reset()
    {
        Kill();
        Place(mOriginalCell);
        mHighlightedCells.Clear();
    }

    public void Kill()
    {
        if (mCurrentCell.mCurrentPiece.type == "king")
        {
            string winningTeam;
            if (mColor == Color.white) winningTeam = "BLUE";
            else winningTeam = "RED";

            mCurrentCell.mBoard.mEndMessage.text = winningTeam + " has won!";
            mCurrentCell.mBoard.mEndMessage.enabled = true;
        }

        mCurrentCell.mCurrentPiece = null;
        gameObject.SetActive(false);
    }

    #region Movement
    private void CreateCellPath(int xDirection, int yDirection)
    {
        //TargetPosition
        int currentX = mCurrentCell.mBoardPosition.x;
        int currentY = mCurrentCell.mBoardPosition.y;

        //Check each cell
        currentX += xDirection;
        currentY += yDirection;

        CellState cellState = CellState.None;

        //target cell 의 state
        cellState = mCurrentCell.mBoard.ValidateCell(currentX, currentY, this);

        if (cellState == CellState.Enemy)
        {
            //mHighlightedCells.Add(mCurrentCell.mBoard.mAllCells[currentX, currentY]);
            return;
        }

        if (cellState != CellState.Free)
            return;
        //Get the state of the target cell

        //Add to list
        mHighlightedCells.Add(mCurrentCell.mBoard.mAllCells[currentX, currentY]);
        
    }

    public virtual void CheckPathing()
    {
        //Horizontal
        CreateCellPath(1, 0);
        CreateCellPath(-1, 0);

        //Vertical
        CreateCellPath(0, 1);
        CreateCellPath(0, -1);

        //Upper diagonal
        CreateCellPath(1, 1);
        CreateCellPath(-1, 1);

        //Lower diagonal
        CreateCellPath(-1, -1);
        CreateCellPath(1, -1);
    }

    protected void ShowCells()
    {
        foreach (Cell cell in mHighlightedCells)
        {
            cell.mOutlineImage.enabled = true;
        }

    }

    public void ClearCells()
    {
        foreach (Cell cell in mHighlightedCells)
        {
            cell.mOutlineImage.enabled = false;
        }
        HideArrow();
        mHighlightedCells.Clear();
    }

    protected virtual void Move()
    {
        // if there is an enemy piece, remove it
        mTargetCell.RemovePiece();

        //Clear current
        mCurrentCell.mCurrentPiece = null;

        //Switch cells
        mCurrentCell = mTargetCell;
        mCurrentCell.mCurrentPiece = this;

        //Move on board
        transform.position = mCurrentCell.transform.position;
        Debug.Log("Move on board: " + transform.position);
        mTargetCell = null;
    }
    #endregion

    #region Rotation

    protected void ShowArrow()
    {
        BasePiece currentpiece = mCurrentCell.mCurrentPiece;
        if ((mColor == Color.black) && (currentpiece.type == "laser") && (currentpiece.direction == 2))
        {
            mCurrentCell.mRotationImageLeft.enabled = true;
            return;
        }
        if ((mColor == Color.black) && (currentpiece.type == "laser") && (currentpiece.direction == 1))
        {
            mCurrentCell.mRotationImageRight.enabled = true;
            return;
        }
        if ((mColor == Color.white) && (currentpiece.type == "laser") && (currentpiece.direction == 0))
        {
            mCurrentCell.mRotationImageLeft.enabled = true;
            return;
        }
        if ((mColor == Color.white) && (currentpiece.type == "laser") && (currentpiece.direction == 3))
        {
            mCurrentCell.mRotationImageRight.enabled = true;
            return;
        }
        mCurrentCell.mRotationImageRight.enabled = true;
        mCurrentCell.mRotationImageLeft.enabled = true;
    }

    protected void HideArrow()
    {
        mCurrentCell.mRotationImageRight.enabled = false;
        mCurrentCell.mRotationImageLeft.enabled = false;
    }

    public void RightRotation()
    {
        mCurrentCell.mCurrentPiece.transform.rotation = Quaternion.AngleAxis(90, Vector3.up);
    }

    public void  LeftRotation()
    {
        mCurrentCell.mCurrentPiece.transform.rotation = Quaternion.AngleAxis(-90, Vector3.up);
    }
    #endregion

    #region Events
    public override void OnPointerClick(PointerEventData eventData)
    {
        base.OnPointerClick(eventData);

        if (click == false)
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (mCurrentCell.mBoard.mAllCells[i, j].mCurrentPiece != null)
                    {
                        mCurrentCell.mBoard.mAllCells[i, j].mCurrentPiece.click = false;
                        mCurrentCell.mBoard.mAllCells[i, j].mCurrentPiece.ClearCells();
                    }
                }
            }

            CheckPathing();

            ShowCells();

            ShowArrow();

            click = true;
        }
        else
        {
            ClearCells();
            click = false;

        }

    }

    public override void OnBeginDrag(PointerEventData eventData)
    {
        base.OnBeginDrag(eventData);

       
    }

    public override void OnDrag(PointerEventData eventData)
    {
        base.OnDrag(eventData);
        
        //Follow Pointer
        transform.position += (Vector3)eventData.delta;
        foreach ( Cell cell in mHighlightedCells)
        {
            if(RectTransformUtility.RectangleContainsScreenPoint(cell.mRectTransform, Input.mousePosition))
            {
                // if the mouse is within a valid cell
                mTargetCell = cell;
                break;
            }

            //mouse is not within any highlighted cell
            mTargetCell = null;
        }
    }

    public override void OnEndDrag(PointerEventData eventData)
    {
        base.OnEndDrag(eventData);

        //return to original position
        if (!mTargetCell)
        {
            transform.position = mCurrentCell.gameObject.transform.position;
            ClearCells();
            return;
        }

        ClearCells();
        Move();
        mPieceManager.SwitchSides(mColor);
    }
    #endregion
}
