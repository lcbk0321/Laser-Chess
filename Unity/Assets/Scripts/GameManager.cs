using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Board mBoard;
    public LaserManager mLaserManager;
    public PieceManager mPieceManager;

    void Start()
    {
        //  create the board
        mBoard.Create();

        //  create lasers
        mLaserManager.Setup(mBoard);

        //  create pieces
        mPieceManager.Setup(mBoard);
    }
    
}
