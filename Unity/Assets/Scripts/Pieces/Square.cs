using UnityEngine;
using UnityEngine.UI;

public class Square : BasePiece
{
    public override void Setup(Color newTeamColor, PieceManager newPieceManager)
    {
        base.Setup(newTeamColor, newPieceManager);

        type = "square";

        if (newTeamColor == Color.black)
        {
            GetComponent<Image>().sprite = Resources.Load<Sprite>("squa_B");
        }
        else
        {
            GetComponent<Image>().sprite = Resources.Load<Sprite>("squa_R");
        }
    }
}

