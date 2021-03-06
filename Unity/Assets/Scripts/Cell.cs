﻿using UnityEngine;
using UnityEngine.UI;

public class Cell : MonoBehaviour
{
    public Image mOutlineImage;
    public Image mRotationImageRight;
    public Image mRotationImageLeft;

    [HideInInspector]
    public Vector2Int mBoardPosition = Vector2Int.zero;
    [HideInInspector]
    public Board mBoard = null;
    [HideInInspector]
    public RectTransform mRectTransform = null;

    [HideInInspector]
    public BasePiece mCurrentPiece = null;

    public void Setup(Vector2Int newBoardPosition, Board newBoard)
    {
        mBoardPosition = newBoardPosition;
        mBoard = newBoard;
        mOutlineImage.enabled = false;

        mRectTransform = GetComponent<RectTransform>();
    }

    public void RemovePiece()
    {
        if(mCurrentPiece != null)
        {
            mCurrentPiece.Kill();
        }
    }
}
