using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public enum PieceType
{

}


public abstract class BasePiece : EventTrigger
{
    [HideInInspector]
    public Color mColor = Color.clear;
    public bool mIsFirstMove = true;

    protected Cell mOriginalCell = null;
    protected Cell mCurrentCell = null;

    protected RectTransform mRectTransform = null;
    protected PieceManager mPieceManager;

    protected Cell mTargetCell = null;

    protected Vector3Int mMovement = Vector3Int.one;
    protected List<Cell> mHighlightedCells = new List<Cell>();

    public void Setup(Color newTeamColor, Color32 newSpriteColor, PieceManager newPieceManager)
    {
        mPieceManager = newPieceManager;
        mColor = newTeamColor;
        GetComponent<Image>().color = newSpriteColor;
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
    }

    public void Kill()
    {
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
            mHighlightedCells.Add(mCurrentCell.mBoard.mAllCells[currentX, currentY]);
            return;
        }

        if (cellState != CellState.Free)
            return;
        //Get the state of the target cell

        //Add to list
        mHighlightedCells.Add(mCurrentCell.mBoard.mAllCells[currentX, currentY]);
        
    }

    public void CheckPathing()
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

    protected void ClearCells()
    {

        foreach (Cell cell in mHighlightedCells)
        {
            cell.mOutlineImage.enabled = false;
        }

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

    #region Events
    public override void OnPointerClick(PointerEventData eventData)
    {
        base.OnPointerClick(eventData);


    }

    public override void OnBeginDrag(PointerEventData eventData)
    {
        base.OnBeginDrag(eventData);


        CheckPathing();

        ShowCells();
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

        Move();
        Debug.Log(mColor);
        mPieceManager.SwitchSides(mColor);
        ClearCells();

        Vector2Int startpoint = new Vector2Int(mCurrentCell.mBoardPosition.x, mCurrentCell.mBoardPosition.y);
        Vector2Int direction = new Vector2Int(0, 1);

        // shoot the laser
        ShootLaser(startpoint, direction);
    }


    // wait a second / show the laser / destroy, 
    public void ShootLaser(Vector2Int startpoint, Vector2Int direction)
    {
        // show the laser
        List<Vector2Int> listToDestroy = ShowLaserPath(startpoint, direction);

        // wait a second


        // if end or destroy
        DestroyPieces(listToDestroy);
    }

    // calculate the algorithm and show only the wanted lasers
    public List<Vector2Int> ShowLaserPath(Vector2Int startpoint, Vector2Int direction)
    {
        List<Vector2Int> listToDestroy = new List<Vector2Int>();

        // ========= 여기에서 ValidateCell 같은 method 를 BasePiece에 만들어줘야 한다. 
        // ========= x, y 를 주면, 그 pieceType 이 나오고, 그 Type 에 따라서 레이져의 행방을 정한다. 
        // Type 에는 적아 구분이 상관 없고, None, Free, OutofBound 를 기본으로, 
        // Splitter 는 laser를 거기서 멈추고 분리. 
        // Square 와 Triangle 은 laser를 거기서 멈추고, 방향에 따라 새로운 레이져를 부를지, 파괴에 저장할지.
        // Laser 는 거기서 멈추기
        // King 은 거기서 멈추고, 파괴에 저장. 


        return listToDestroy;
    }

    // destory pieces in the list: if undestroyable, do not. if king, game end.
    public void DestroyPieces(List<Vector2Int> listToDestroy)
    {
        
    }
    #endregion
}
