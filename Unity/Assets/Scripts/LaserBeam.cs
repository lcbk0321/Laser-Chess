using UnityEngine;
using UnityEngine.UI;

public class LaserBeam : MonoBehaviour
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

        mLaserImage.enabled = false;
    }

    public void EnableLaser()
    {
        mLaserImage.enabled = true;
    }

    public void DisableLaser()
    {
        mLaserImage.enabled = false;
    }

}
