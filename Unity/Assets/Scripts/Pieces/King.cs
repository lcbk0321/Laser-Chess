using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class King : BasePiece
{
    public override void Setup(Color newTeamColor, PieceManager newPieceManager)
    {
        base.Setup(newTeamColor, newPieceManager);

        type = "king";

        /*if (newTeamColor == Color.black)
        {
            GetComponent<Image>().sprite = Resources.Load<Sprite>("king_B");
        }
        else
        {
            GetComponent<Image>().sprite = Resources.Load<Sprite>("king_R");
        }*/
    }
}
