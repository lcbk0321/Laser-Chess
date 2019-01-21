using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PieceManager : MonoBehaviour
{
    [HideInInspector]
    public bool mIsKingAlive = true;

    public Board mBoard;

    public bool firstSetup = true;

    public GameObject mPiecePrefab;
    public Text mturn;
    private List<BasePiece> mWhitePieces = null;
    private List<BasePiece> mBlackPieces = null;

    private string[] mPieceOrder = new string[10]
    {
        "L", "S", "K", "S", "T", "T", "T","T", "SP", "T"
    };

    public List<Vector2Int> listToDestroy;

    private Dictionary<string, Type> mPieceLibrary = new Dictionary<string, Type>()
    {
        {"SP",  typeof(Splitter)},
        {"S",  typeof(Square)},
        {"T",  typeof(Triangle)},
        {"L",  typeof(Laser)},
        {"K",  typeof(King)}
    };

    public void Setup(Board board)
    {        
        mWhitePieces = CreatePieces(Color.white, board);

        mBlackPieces = CreatePieces(Color.black, board);

        PlacePieces(mWhitePieces, mBlackPieces, board);

        mBoard = board;

        //White goes first
        SwitchSides(Color.black);
    }

    private List<BasePiece> CreatePieces(Color teamColor, Board board)
    {
        List<BasePiece> newPieces = new List<BasePiece> ();

        for ( int i = 0; i < mPieceOrder.Length; i++)
        {
            //Create new object
            GameObject newPieceObject = Instantiate(mPiecePrefab);
            newPieceObject.transform.SetParent(transform);

            //Set scale and position
            newPieceObject.transform.localScale = new Vector3(1, 1, 1);
            newPieceObject.transform.localRotation = Quaternion.identity;

            //Get the type, apply to new object
            string key = mPieceOrder[i];
            Type pieceType = mPieceLibrary[key];

            //Store new piece
            BasePiece newPiece = (BasePiece) newPieceObject.AddComponent(pieceType);
            newPieces.Add(newPiece);

            //Setup piece
            newPiece.Setup(teamColor, this);
        }
        return newPieces;
    }

    private BasePiece CreatePiece(Type pieceType)
    {
        return null;
    }

    private void PlacePieces(List<BasePiece> whitepieces, List<BasePiece> blackpieces, Board board)
    {
        whitepieces[0].Place(board.mAllCells[7, 7], null);
        blackpieces[0].Place(board.mAllCells[0, 0], null);
        

        for (int i = 1; i < 5; i++)
        {
            whitepieces[i].Place(board.mAllCells[7, 6-i], null);
            blackpieces[i].Place(board.mAllCells[0, i+1], null);
        }

        for (int i = 5; i < 7; i++)
        {
            whitepieces[i].Place(board.mAllCells[4, i-2], null);
            blackpieces[i].Place(board.mAllCells[3, i-2], null);
        }
        whitepieces[4].transform.rotation = Quaternion.AngleAxis(90, Vector3.forward);
        blackpieces[4].transform.rotation = Quaternion.AngleAxis(90, Vector3.forward);
        whitepieces[5].transform.rotation=Quaternion.AngleAxis(90, Vector3.forward);
        blackpieces[6].transform.rotation = Quaternion.AngleAxis(90, Vector3.forward);
        whitepieces[7].Place(board.mAllCells[3, 7], null);
        blackpieces[7].Place(board.mAllCells[4, 0], null);
        whitepieces[7].transform.rotation = Quaternion.AngleAxis(180, Vector3.forward);
        blackpieces[7].transform.rotation = Quaternion.AngleAxis(180, Vector3.forward);
        whitepieces[8].Place(board.mAllCells[0, 7], null);
        blackpieces[8].Place(board.mAllCells[7, 0], null);
        whitepieces[9].Place(board.mAllCells[0, 1], null);
        blackpieces[9].Place(board.mAllCells[7, 6], null);
        whitepieces[9].transform.rotation = Quaternion.AngleAxis(180, Vector3.forward);
        blackpieces[9].transform.rotation = Quaternion.AngleAxis(180, Vector3.forward);

        for(int i = 0; i < 10; i ++)
        {
            whitepieces[i].mOriginalTransform = whitepieces[i].mRectTransform;
        }

        //direction
        // ---- black ---- //
        blackpieces[0].direction = 2;
        blackpieces[1].direction = 1;
        blackpieces[2].direction = 1;
        blackpieces[3].direction = 1;
        blackpieces[4].direction = 1;
        blackpieces[5].direction = 2;
        blackpieces[6].direction = 1;
        blackpieces[7].direction = 0;
        blackpieces[8].direction = 1;
        blackpieces[9].direction = 0;
        // ---- white ----
        whitepieces[0].direction = 0;
        whitepieces[1].direction = 3;
        whitepieces[2].direction = 3;
        whitepieces[3].direction = 3;
        whitepieces[4].direction = 3;
        whitepieces[5].direction = 3;
        whitepieces[6].direction = 0;
        whitepieces[7].direction = 2;
        whitepieces[8].direction = 1;
        whitepieces[9].direction = 2;
    }

    private void SetInteractive(List<BasePiece> allPieces, bool value)
    {
        foreach(BasePiece piece in allPieces)
        {
            piece.enabled = value;
        }
    }

    public void SwitchSides(Color color)
    {
        Vector2Int startpoint;
        int direction;

        if (!mIsKingAlive)
        {
            //Reset pieces
            ResetPieces();

            //King state is alive again
            mIsKingAlive = true;

            //  white is first again
            color = Color.black;
        }

        bool isBlackTurn = color == Color.white ? true : false;
        if (color == Color.black)
        {
            mturn.text = "RED";
            mturn.color = new Color32(210, 95, 64, 255);

        }
        else
        {
            mturn.text = "BLUE";
            mturn.color = new Color32(80, 124, 159, 255);
        }

        // LASER Codes //
        bool ifWhiteTeam = (color == Color.white);

        if (ifWhiteTeam)
        {
            Debug.Log("Moved White Team");
            startpoint = new Vector2Int(7, 7);
            direction = mBoard.mAllCells[7, 7].mCurrentPiece.direction;
        }
        else
        {
            Debug.Log("Moved Black Team");
            startpoint = new Vector2Int(0, 0);
            direction = mBoard.mAllCells[0, 0].mCurrentPiece.direction;
        }

        if (!firstSetup)
        {
            // shoot the laser
            ShootLaser(startpoint, direction);
        }

        // Set interactivity
        SetInteractive(mWhitePieces, !isBlackTurn);
        SetInteractive(mBlackPieces, isBlackTurn);

        firstSetup = false;
    }

    public void ResetPieces()
    {
        Debug.Log("onclick");
        //  Reset white
        foreach ( BasePiece piece in mWhitePieces)
        {
            piece.Reset();
        }
        
        //  Reset Black
        foreach (BasePiece piece in mBlackPieces)
        {
            piece.Reset();
        }
    }

    // ================================================================================= //
    // =========================== !! LASER Codes !! =================================== //
    // ================================================================================= //
    // ================================================================================= //
    // wait a second / show the laser / destroy, 
    public void ShootLaser(Vector2Int startpoint, int direction)
    {
        Debug.Log("ShootLaser startpoint: " + startpoint.x + ", " + startpoint.y);
        Debug.Log("ShootLaser direction: " + direction);
        // show the laser
        listToDestroy = ShowLaserPath(startpoint, direction);

        // if end or destroy
        Invoke("DestroyPieces", 1);
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
        Debug.Log("direction: " + direction);

        CellState cellState = CellState.None;
        cellState = mBoard.ValidateLaser(targetX, targetY); // check if laser to go on. 

        BasePiece meetPiece = null;

        // check if laser is horizontal or vertical
        int HorizontalVertical; // horizontal: 0, vertical: 1
        if (dirVector.y == 0)
            HorizontalVertical = 0;
        else
            HorizontalVertical = 1;
        switch (cellState)
        {
            case CellState.OutOfBounds: // just end the function
                break;
            case CellState.Free: // keep going on.
                Debug.Log("CellState.Free");
                /*============ show this laser============*/
                if (HorizontalVertical == 0) // horizontal path y == 0
                {
                    if (dirVector.x == 1)
                        mBoard.mAllLaserBeams[startpoint.x, startpoint.y, 0].EnableLaser();
                    else // opposite
                        mBoard.mAllLaserBeams[(startpoint.x - 1), startpoint.y, 0].EnableLaser();
                }
                else // vertical path x == 0
                {
                    if (dirVector.y == 1)
                        mBoard.mAllLaserBeams[startpoint.y, startpoint.x, 1].EnableLaser();
                    else
                    {
                        Debug.Log("draw x: " + (startpoint.y - 1) + ", y: " + startpoint.x);
                        mBoard.mAllLaserBeams[(startpoint.y - 1), startpoint.x, 1].EnableLaser();
                    }
                }
                // go on
                List<Vector2Int> moreListToDetroy = ShowLaserPath(mTargetPosition, direction);
                listToDestroy.AddRange(moreListToDetroy);
                break;
            case CellState.Friendly: // 실제 아군이 아니라, 적/아 구분 상관 없음
                Debug.Log("CellState.Friendly");
                /*============ show this laser============*/
                if (HorizontalVertical == 0) // horizontal path y == 0
                {
                    if (dirVector.x == 1)
                        mBoard.mAllLaserBeams[startpoint.x, startpoint.y, 0].EnableLaser();
                    else // opposite
                        mBoard.mAllLaserBeams[(startpoint.x - 1), startpoint.y, 0].EnableLaser();
                }
                else // vertical path x == 0
                {
                    if (dirVector.y == 1)
                        mBoard.mAllLaserBeams[startpoint.y, startpoint.x, 1].EnableLaser();
                    else
                        mBoard.mAllLaserBeams[(startpoint.y - 1), startpoint.x, 1].EnableLaser();
                }
                meetPiece = mBoard.mAllCells[targetX, targetY].mCurrentPiece;
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
            Debug.Log("meetPiece direction: " + mPieceDirection);
            Debug.Log("meetPiece type: " + meetPiece.type);
            switch (meetPiece.type)
            {
                case "splitter":
                    List<Vector2Int> afterSplitterOne, afterSplitterTwo;
                    afterSplitterOne = ShowLaserPath(mTargetPosition, direction);
                    if ((mPieceDirection % 2) == 0)  // if splitter is in '/' direction
                    {
                        if ((direction == 0) || (direction == 1)) // laser left, up
                            afterSplitterTwo = ShowLaserPath(mTargetPosition, vecToDir(dirVector + One_mOne));
                        else // laser right, down
                            afterSplitterTwo = ShowLaserPath(mTargetPosition, vecToDir(dirVector + mOne_One));
                    }
                    else  // if splitter is opposite to '/'
                    {
                        if ((direction == 0) || (direction == 3)) // laser left, up
                            afterSplitterTwo = ShowLaserPath(mTargetPosition, vecToDir(dirVector + One_One));
                        else // laser right, down
                            afterSplitterTwo = ShowLaserPath(mTargetPosition, vecToDir(dirVector + mOne_mOne));
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
                        listToDestroy.Add(mTargetPosition);
                    break;
                case "triangle":
                    List<Vector2Int> afterTriangle = new List<Vector2Int>();
                    if (mPieceDirection == 0)
                    {
                        if ((direction == 0) || (direction == 1)) // die if left, up
                            listToDestroy.Add(mTargetPosition);
                        else // reflection if right, down
                            afterTriangle = ShowLaserPath(mTargetPosition, vecToDir(dirVector + mOne_One));
                    }
                    else if (mPieceDirection == 1)
                    {
                        if ((direction == 1) || (direction == 2)) // die if up, right
                            listToDestroy.Add(mTargetPosition);
                        else // reflection
                            afterTriangle = ShowLaserPath(mTargetPosition, vecToDir(dirVector + One_One));
                    }
                    else if (mPieceDirection == 2)
                    {
                        if ((direction == 2) || (direction == 3)) // die if right, down
                            listToDestroy.Add(mTargetPosition);
                        else // reflection
                            afterTriangle = ShowLaserPath(mTargetPosition, vecToDir(dirVector + One_mOne));
                    }
                    else // mPieceDirection == 3
                    {
                        if ((direction == 3) || (direction == 0)) // die if down, left
                            listToDestroy.Add(mTargetPosition);
                        else // reflection
                            afterTriangle = ShowLaserPath(mTargetPosition, vecToDir(dirVector + mOne_mOne));
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
    public void DestroyPieces()  // List<Vector2Int> listToDestroy
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
            mBoard.mAllCells[destroyX, destroyY].RemovePiece();
        }

        //// set time out
        //// wait a second
        //System.Threading.Thread.Sleep(1000);

        // hide the laserpath
        for (int i = 0; i < 7; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                for (int k = 0; k < 2; k++)
                {
                    mBoard.mAllLaserBeams[i, j, k].DisableLaser();
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
}
