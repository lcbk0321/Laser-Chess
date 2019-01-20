using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class rotationRight : MonoBehaviour
{
    public GameObject ParentGameObject;
    private BasePiece currentpiece;
    private Image right;
    private Image left;

    // Start is called before the first frame update
    void Start()
    {
        currentpiece = ParentGameObject.GetComponent<Cell>().mCurrentPiece;
        
        right = ParentGameObject.GetComponent<Cell>().mRotationImageRight;
        left = ParentGameObject.GetComponent<Cell>().mRotationImageLeft;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Click()
    {
        Debug.Log("Click");

        foreach (Cell cell in currentpiece.mHighlightedCells)
        {
            cell.mOutlineImage.enabled = false;
        }
        currentpiece.transform.Rotate(0.0f, 0.0f, -90.0f);
        right.enabled = false;
        left.enabled = false;
        currentpiece.mPieceManager.SwitchSides(currentpiece.mColor);
    }
}

