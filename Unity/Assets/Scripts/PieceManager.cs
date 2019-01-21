using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PieceManager : MonoBehaviour
{
    [HideInInspector]
    public bool mIsKingAlive = true;

    public GameObject mPiecePrefab;
    public Text mturn;
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
        blackpieces[0].direction = 2;
        blackpieces[1].direction = 3;
        blackpieces[2].direction = 3;
        blackpieces[3].direction = 3;
        blackpieces[4].direction = 0;
        blackpieces[5].direction = 0;
        blackpieces[6].direction = 3;
        blackpieces[7].direction = 2;
        blackpieces[8].direction = 1;
        blackpieces[9].direction = 2;
        whitepieces[0].direction = 2;
        whitepieces[1].direction = 1;
        whitepieces[2].direction = 1;
        whitepieces[3].direction = 1;
        whitepieces[4].direction = 2;
        whitepieces[5].direction = 2;
        whitepieces[6].direction = 1;
        whitepieces[7].direction = 0;
        whitepieces[8].direction = 1;
        whitepieces[9].direction = 0;
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
        //  Set interactivity
        SetInteractive(mWhitePieces, !isBlackTurn);
        SetInteractive(mBlackPieces, isBlackTurn);
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
}
