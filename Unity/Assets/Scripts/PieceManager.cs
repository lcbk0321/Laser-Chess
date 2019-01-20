using System;
using System.Collections.Generic;
using UnityEngine;

public class PieceManager : MonoBehaviour
{
    [HideInInspector]
    public bool mIsKingAlive = true;

    public GameObject mPiecePrefab;

    private List<BasePiece> mWhitePieces = null;
    private List<BasePiece> mBlackPieces = null;

    private string[] mPieceOrder = new string[10]
    {
        "L", "S", "K", "S", "T", "T", "T","T", "SP", "T"
    };

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

            //Stroe new piece
            BasePiece newPiece = (BasePiece)newPieceObject.AddComponent(pieceType);
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
        whitepieces[0].Place(board.mAllCells[7, 7]);
        blackpieces[0].Place(board.mAllCells[0, 0]);

        for (int i = 1; i < 5; i++)
        {
            whitepieces[i].Place(board.mAllCells[7, 6-i]);
            blackpieces[i].Place(board.mAllCells[0, i+1]);
        }

        for (int i = 5; i < 7; i++)
        {
            whitepieces[i].Place(board.mAllCells[4, i-2]);
            blackpieces[i].Place(board.mAllCells[3, i-2]);
        }
        whitepieces[5].transform.rotation=Quaternion.AngleAxis(90, Vector3.forward);
        blackpieces[6].transform.rotation = Quaternion.AngleAxis(90, Vector3.forward);
        whitepieces[7].Place(board.mAllCells[3, 7]);
        blackpieces[7].Place(board.mAllCells[4, 0]);
        whitepieces[7].transform.rotation = Quaternion.AngleAxis(180, Vector3.forward);
        blackpieces[7].transform.rotation = Quaternion.AngleAxis(180, Vector3.forward);
        whitepieces[8].Place(board.mAllCells[0, 7]);
        blackpieces[8].Place(board.mAllCells[7, 0]);
        whitepieces[9].Place(board.mAllCells[0, 1]);
        blackpieces[9].Place(board.mAllCells[7, 6]);
        whitepieces[9].transform.rotation = Quaternion.AngleAxis(180, Vector3.forward);
        blackpieces[9].transform.rotation = Quaternion.AngleAxis(180, Vector3.forward);
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

        //  Set interactivity
        SetInteractive(mWhitePieces, !isBlackTurn);
        SetInteractive(mBlackPieces, isBlackTurn);
    }

    public void ResetPieces()
    {
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

}
