using UnityEngine;

public class LaserManager : MonoBehaviour
{
    public GameObject mLaserBeamPrefab;

    public Board mBoard;

    // Setup called from GameManager
    public void Setup(Board board)
    {
        mBoard = board;
        CreateLaserBeams();
    }

    // draw LaserBeams
    public void CreateLaserBeams()
    {
        // horizontal
        for (int y = 0; y < 8; y++)
        {
            for (int x = 0; x < 7; x++)
            {
                // Create the laserbeams
                GameObject newLaserBeam = Instantiate(mLaserBeamPrefab);
                newLaserBeam.transform.SetParent(transform);

                // Set laserbeam position
                RectTransform rectTransform = newLaserBeam.GetComponent<RectTransform>();
                rectTransform.localScale = new Vector3(1.2f, 2, 1);
                rectTransform.anchoredPosition = new Vector2((x * 100 - 300), (y * 100 - 350));

                // Setup 
                mBoard.mAllLaserBeams[x, y, 0] = newLaserBeam.GetComponent<LaserBeam>();
                mBoard.mAllLaserBeams[x, y, 0].Setup(x, y, 0);
            }
        }

        // vertical
        for (int y = 0; y < 8; y++)
        {
            for (int x = 0; x < 7; x++)
            {
                // Create the laser
                GameObject newLaserBeam = Instantiate(mLaserBeamPrefab);
                newLaserBeam.transform.SetParent(transform);

                // Set laser position
                RectTransform rectTransform = newLaserBeam.GetComponent<RectTransform>();
                rectTransform.localScale = new Vector3(1.2f, 2, 1);
                rectTransform.rotation = Quaternion.AngleAxis(90, Vector3.forward);
                rectTransform.anchoredPosition = new Vector2((y * 100 - 350), (x * 100 - 300));

                // Setup 
                mBoard.mAllLaserBeams[x, y, 1] = newLaserBeam.GetComponent<LaserBeam>();
                mBoard.mAllLaserBeams[x, y, 1].Setup(x, y, 1);

                // ================================================================================== //
                // ...
                // (2, 0)
                // (1, 0)
                // (0, 0) (0, 1) ...
                // ================================================================================== //
            }
        }
    }




}
   