using UnityEngine;
using UnityEngine.UI;

public class Laser : BasePiece
{
    public override void Setup(Color newTeamColor, PieceManager newPieceManager)
    {
        base.Setup(newTeamColor, newPieceManager);

        type = "laser";

        if (newTeamColor == Color.black)
        {
            GetComponent<Image>().sprite = Resources.Load<Sprite>("laser_B");
        }
        else
        {
            GetComponent<Image>().sprite = Resources.Load<Sprite>("laser_R");
        }       
    }
}

