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

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Click()
    {
        Debug.Log("Click");
        currentpiece = ParentGameObject.GetComponent<Cell>().mCurrentPiece;
        right = ParentGameObject.GetComponent<Cell>().mRotationImageRight;
        left = ParentGameObject.GetComponent<Cell>().mRotationImageLeft;
        if (currentpiece.direction + 1 > 3)
        {
            currentpiece.direction = currentpiece.direction + 1 - 4;
        }
        else
        {
            currentpiece.direction++;
        }
        currentpiece.transform.Rotate(0.0f, 0.0f, -90.0f);
        right.enabled = false;
        left.enabled = false;
        Debug.Log("before:" + currentpiece.mHighlightedCells.Count);
        currentpiece.ClearCells();
        Debug.Log("after:" + currentpiece.mHighlightedCells.Count);
        currentpiece.mPieceManager.SwitchSides(currentpiece.mColor);
    }
}

