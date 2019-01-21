using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;


public abstract class BasePiece : EventTrigger
{
    [HideInInspector]
    public bool click = false;
    public Color mColor = Color.clear;
    public bool mIsFirstMove = true;
    //public string type = null;
    private Canvas canv;

    public int direction = 0;
    public string type = null;

    protected Cell mOriginalCell = null;
    public RectTransform mOriginalTransform = null;
    protected Cell mCurrentCell = null;

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

    public void Place(Cell newCell, RectTransform originaltransform)
    {
        //Cell Stuff
        mCurrentCell = newCell;
        mOriginalCell = newCell;
        mCurrentCell.mCurrentPiece = this;

        //Object stuff
        transform.position = newCell.transform.position;
        if (originaltransform != null)
        {
            transform.rotation = originaltransform.rotation;
        }
        Debug.Log("Place function in BasePiece: " + transform.position);
        
        Debug.Log(transform.position.x+", "+ transform.position.y);

        gameObject.SetActive(true);
    }

    public void Reset()
    {
        Kill();
        Place(mOriginalCell, mOriginalTransform);
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

    protected void ClearCells()
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
        Vector2Int startpoint;
        int direction;

        Destroy(canv);
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
        

        bool ifWhiteTeam = (mCurrentCell.mCurrentPiece.mColor == Color.white);

        if (ifWhiteTeam)
        {
            Debug.Log("Moved White Team");
            startpoint = new Vector2Int(7, 7);
            direction = mCurrentCell.mBoard.mAllCells[7, 7].mCurrentPiece.direction;
        }
        else
        {
            Debug.Log("Moved Black Team");
            startpoint = new Vector2Int(0, 0);
            direction = mCurrentCell.mBoard.mAllCells[0, 0].mCurrentPiece.direction;
        }

        // wait a second
        //System.Threading.Thread.Sleep(1000);

        // shoot the laser
        ShootLaser(startpoint, direction);
    }

    // wait a second / show the laser / destroy, 
    public void ShootLaser(Vector2Int startpoint, int direction)
    {
        Debug.Log("ShootLaser startpoint: " + startpoint.x + ", " + startpoint.y);
        Debug.Log("ShootLaser direction: " + direction);
        // show the laser
        List<Vector2Int> listToDestroy = ShowLaserPath(startpoint, direction);

        // wait a second
        //System.Threading.Thread.Sleep(5000);

        // if end or destroy
        DestroyPieces(listToDestroy);
    }

    // calculate the algorithm and show only the wanted lasers
    public List<Vector2Int> ShowLaserPath(Vector2Int startpoint, int direction)
    {
        List<Vector2Int> listToDestroy = new List<Vector2Int>();
        Vector2Int dirVector;

        switch (direction)
        {
            case 0:
                dirVector = Vector2Int.left;    // new Vector2Int(-1, 0);
                break;
            case 1:
                dirVector = Vector2Int.up;      // new Vector2Int(0, 1);
                break;
            case 2:
                dirVector = Vector2Int.right;   // new Vector2Int(1, 0);
                break;
            case 3:
                dirVector = Vector2Int.down;    // new Vector2Int(0, -1);
                break;
            default:
                dirVector = Vector2Int.zero;    // new Vector2Int(0, 0);
                break;
        }

        Vector2Int mTargetPosition = startpoint + dirVector;
        int targetX = mTargetPosition.x;
        int targetY = mTargetPosition.y;
        Debug.Log("mTargetPosition.x: " + targetX);
        Debug.Log("mTargetPosition.y: " + targetY);

        CellState cellState = CellState.None;
        cellState = mCurrentCell.mBoard.ValidateLaser(targetX, targetY); // check if laser to go on. 

        BasePiece meetPiece = null;

        // check if laser is horizontal or vertical
        int ifHorizontal; // horizontal: 0, vertical: 1
        if (dirVector.y == 0)
            ifHorizontal = 0;
        else
            ifHorizontal = 1;
        switch (cellState)
        {
            case CellState.OutOfBounds: // just end the function
                break;
            case CellState.Free: // keep going on.
                /*============ show this laser============*/
                if (ifHorizontal == 0) // horizontal path
                {
                    if (dirVector.x == 1)
                        mCurrentCell.mBoard.mAllLaserBeams[startpoint.x, startpoint.y, 0].EnableLaser();
                    else // opposite
                        mCurrentCell.mBoard.mAllLaserBeams[(startpoint.x-1), startpoint.y, 0].EnableLaser();
                }
                else // vertical path
                {
                    if (dirVector.y == 1)
                        mCurrentCell.mBoard.mAllLaserBeams[startpoint.y, startpoint.x, 1].EnableLaser();
                    else
                        mCurrentCell.mBoard.mAllLaserBeams[startpoint.y, (startpoint.x-1), 0].EnableLaser();
                }
                // go on
                List<Vector2Int> moreListToDetroy = ShowLaserPath(mTargetPosition, direction);
                listToDestroy.AddRange(moreListToDetroy);
                break;
            case CellState.Friendly: // 실제 아군이 아니라, 적/아 구분 상관 없음
                /*============ show this laser============*/
                meetPiece = mCurrentCell.mBoard.mAllCells[targetX, targetY].mCurrentPiece;
                break;
            default:
                break;
        }

        // for the case of CellState.Friendly => "무언가를 만났다"
        if (meetPiece != null)
        {
            // 4가지 삼각형 방향에 대한 법선 벡터
            Vector2Int One_One = new Vector2Int(1, 1);
            Vector2Int mOne_One = new Vector2Int(-1, 1);
            Vector2Int One_mOne = new Vector2Int(1, -1);
            Vector2Int mOne_mOne = new Vector2Int(-1, -1);

            // ============================================================================================== //
            // meetPiece => 5가지 말들의 종류에 따라서
            // LaserBeam 은 DEFAULT 로 무언가를 만나면 멈춘다. 
            // Splitter : 두 ShowLaserPath 생성. 
            // Square/Triangle : 방향에 따라 새로운 레이져를 부를지, 파괴에 저장할지.
            // Laser : 그냥 끝!
            // ============================================================================================== //
            int mPieceDirection = meetPiece.direction; // 무언가의 모양 direction
            switch (meetPiece.type)
            {
                case "splitter":
                    List<Vector2Int> afterSplitterOne, afterSplitterTwo;
                    afterSplitterOne = ShowLaserPath(mTargetPosition, direction);
                    if ((mPieceDirection%2) == 0)  // if splitter is in '/' direction
                    {
                        if ((direction == 0)||(direction == 1)) // laser left, up
                        {
                            afterSplitterTwo = ShowLaserPath(mTargetPosition, vecToDir(dirVector+One_mOne));
                        }
                        else // laser right, down
                        {
                            afterSplitterTwo = ShowLaserPath(mTargetPosition, vecToDir(dirVector+mOne_One));
                        }
                    }
                    else  // if splitter is opposite to '/'
                    {
                        if ((direction == 0) || (direction == 3)) // laser left, up
                        {
                            afterSplitterTwo = ShowLaserPath(mTargetPosition, vecToDir(dirVector + One_One));
                        }
                        else // laser right, down
                        {
                            afterSplitterTwo = ShowLaserPath(mTargetPosition, vecToDir(dirVector + mOne_mOne));
                        }
                    }
                    listToDestroy.AddRange(afterSplitterOne);
                    listToDestroy.AddRange(afterSplitterTwo);
                    break;
                case "square":
                    // if reflect
                    if (mPieceDirection == ((direction + 1) % 4))
                    {
                        List<Vector2Int> afterSquare = ShowLaserPath(mTargetPosition, vecToDir(Vector2Int.zero - dirVector));
                        listToDestroy.AddRange(afterSquare);
                    }
                    else // if die
                    {
                        listToDestroy.Add(mTargetPosition);
                    }
                    break;
                case "triangle":
                    List<Vector2Int> afterTriangle = new List<Vector2Int>();
                    if (mPieceDirection == 0)
                    {
                        if ((direction == 0)||(direction == 1)) // die if left, up
                        {
                            listToDestroy.Add(mTargetPosition);
                        }
                        else // reflection if right, down
                        {
                            afterTriangle = ShowLaserPath(mTargetPosition, vecToDir(dirVector + mOne_One));
                        }
                    }
                    else if (mPieceDirection == 1)
                    {
                        if ((direction == 1) || (direction == 2)) // die if up, right
                        {
                            listToDestroy.Add(mTargetPosition);
                        }
                        else // reflection
                        {
                            afterTriangle = ShowLaserPath(mTargetPosition, vecToDir(dirVector + One_One));
                        }
                    }
                    else if (mPieceDirection == 2)
                    {
                        if ((direction == 2) || (direction == 3)) // die if right, down
                        {
                            listToDestroy.Add(mTargetPosition);
                        }
                        else // reflection
                        {
                            afterTriangle = ShowLaserPath(mTargetPosition, vecToDir(dirVector + One_mOne));
                        }
                    }
                    else // mPieceDirection == 3
                    {
                        if ((direction == 3) || (direction == 0)) // die if down, left
                        {
                            listToDestroy.Add(mTargetPosition);
                        }
                        else // reflection
                        {
                            afterTriangle = ShowLaserPath(mTargetPosition, vecToDir(dirVector + mOne_mOne));
                        }
                    }
                    listToDestroy.AddRange(afterTriangle);
                    break;
                case "laser":
                    break;
                case "king":
                    // Append the king to listToDestory
                    listToDestroy.Add(mTargetPosition);
                    break;
                default:
                    break;
            }
            meetPiece = null;
        } // =============================================================================================== //

        return listToDestroy;
    }

    // destory pieces in the list: if undestroyable, do not. if king, game end.
    public void DestroyPieces(List<Vector2Int> listToDestroy)
    {
        // destroy
        for (int i = 0; i < listToDestroy.Count; i++)
        {
            Vector2Int elementToDestroy = listToDestroy[i];
            int destroyX = elementToDestroy.x;
            int destroyY = elementToDestroy.y;
            // ============================================= //
            //           animation would be great!!          //
            // ============================================= //
            mCurrentCell.mBoard.mAllCells[destroyX, destroyY].RemovePiece();
        }
            
        // hide the laserpath
        for (int i=0; i<7; i++)
        {
            for (int j=0; j<8; j++)
            {
                for (int k=0; k<2; k++)
                {
                    mCurrentCell.mBoard.mAllLaserBeams[i, j, k].DisableLaser();
                }
            }
        }

        // reset
        listToDestroy = null;
    }

    public int vecToDir(Vector2Int vector)
    {
        // default left, up, right, down
        Vector2Int left = new Vector2Int(-1, 0);
        Vector2Int up = new Vector2Int(0, 1);
        Vector2Int right = new Vector2Int(1, 0);
        Vector2Int down = new Vector2Int(0, -1);

        if (vector == left) return 0;
        else if (vector == up) return 1;
        else if (vector == right) return 2;
        else if (vector == down) return 3;
        else return -1; // error
    }
    #endregion
}
