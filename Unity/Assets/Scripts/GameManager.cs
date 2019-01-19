using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public Board mBoard;
    public PieceManager mPieceManager;

    void Start()
    {
        //  create the board
        mBoard.Create();

        //  create pieces
        mPieceManager.Setup(mBoard);
    }
}
