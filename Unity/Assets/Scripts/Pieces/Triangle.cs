using UnityEngine;
using UnityEngine.UI;

public class Triangle : BasePiece
{
    public override void Setup(Color newTeamColor, PieceManager newPieceManager)
    {
        base.Setup(newTeamColor, newPieceManager);

        type = "triangle";

        if (newTeamColor == Color.black)
        {
            GetComponent<Image>().sprite = Resources.Load<Sprite>("tri_B");
        }
        else
        {
            GetComponent<Image>().sprite = Resources.Load<Sprite>("tri_R");
        }
    }
}
