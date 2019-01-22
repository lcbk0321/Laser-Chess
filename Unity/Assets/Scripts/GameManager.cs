using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Board mBoard;
    public LaserManager mLaserManager;
    public PieceManager mPieceManager;
    public GameOver mEndMessage;

    void Start()
    {
        //  create the board
        mBoard.Create();

        //  create lasers
        mLaserManager.Setup(mBoard);

        //  create pieces
        mPieceManager.Setup(mBoard);

        //  create message
        // mEndMessage.Setup();
    }

}
