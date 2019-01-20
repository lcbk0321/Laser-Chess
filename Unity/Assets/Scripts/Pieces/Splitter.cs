using UnityEngine;
using UnityEngine.UI;

public class Splitter : BasePiece
{
    public override void Setup(Color newTeamColor, PieceManager newPieceManager)
    {
        base.Setup(newTeamColor, newPieceManager);

        type = "splitter";

        if (newTeamColor == Color.black)
        {
            GetComponent<Image>().sprite = Resources.Load<Sprite>("spl_B");
        }
        else
        {
            GetComponent<Image>().sprite = Resources.Load<Sprite>("spl_R");
        }
    }
}
