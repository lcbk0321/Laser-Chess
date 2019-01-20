using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserManager : MonoBehaviour
{
    public GameObject mLaserPrefab;

    [HideInInspector]
    public Laser[,,] mAllLasers = new Laser[7, 8, 2];

    // Start is called before the first frame update
    public void Setup()
    {
        // horizontal
        for (int y = 0; y < 8; y++)
        {
            for (int x = 0; x < 7; x++)
            {
                // Create the laser
                GameObject newLaser = Instantiate(mLaserPrefab);
                newLaser.transform.SetParent(transform);

                // Set laser position
                RectTransform rectTransform = newLaser.GetComponent<RectTransform>();
                rectTransform.localScale = new Vector3(1.2f, 2, 1);
                rectTransform.anchoredPosition = new Vector2((x * 100 - 300), (y * 100 - 350));

                // Setup 
                mAllLasers[x, y, 0] = newLaser.GetComponent<Laser>();
                mAllLasers[x, y, 0].Setup(x, y, 0);
            }
        }

        // ============ turn image vertically ============ //

        // vertical
        for (int y = 0; y < 8; y++)
        {
            for (int x = 0; x < 7; x++)
            {
                // Create the laser
                GameObject newLaser = Instantiate(mLaserPrefab);
                newLaser.transform.SetParent(transform);    

                // Set laser position
                RectTransform rectTransform = newLaser.GetComponent<RectTransform>();
                rectTransform.localScale = new Vector3(2, 1.2f, 1);
                rectTransform.rotation = Quaternion.AngleAxis(90, Vector3.forward);
                rectTransform.anchoredPosition = new Vector2((y * 100 - 350), (x * 100 - 300));

                // Setup 
                mAllLasers[x, y, 1] = newLaser.GetComponent<Laser>();
                mAllLasers[x, y, 1].Setup(x, y, 1);
            }
        }
    }







}
