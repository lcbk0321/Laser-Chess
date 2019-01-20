using UnityEngine;
using UnityEngine.UI;

public class Laser :MonoBehaviour
{
    public Image mLaserImage;

    [HideInInspector]
    public int mOnTwoCells;
    [HideInInspector]
    public int mWhichLine;
    [HideInInspector]
    public int mZeroIfHorizontal;

    public void Setup(int onTwoCells, int whichLine, int zeroIfHorizontal)
    {
        mOnTwoCells = onTwoCells;
        mWhichLine = whichLine;
        mZeroIfHorizontal = zeroIfHorizontal;

        // mLaserImage.enabled = false;
    }

}
