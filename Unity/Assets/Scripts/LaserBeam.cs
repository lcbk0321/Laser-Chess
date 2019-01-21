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
        Debug.Log("been to EnableLaser() fuction");
        mLaserImage.enabled = true;
    }

    public void DisableLaser()
    {
        Debug.Log("now DisableLaser() now");
        mLaserImage.enabled = false;
    }

}
