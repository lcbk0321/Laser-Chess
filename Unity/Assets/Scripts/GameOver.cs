using UnityEngine;
using UnityEngine.UI;

public class GameOver : MonoBehaviour
{
    public Board mBoard;

    public void Setup()
    {
        Debug.Log("GameEndMessage Setup()");
    }

    public void Start()
    {
        mBoard.mEndMessage.enabled = false;
    }
}
