using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : BasePiece
{

    LineRenderer line;
    public string SortingLayer;
    public int OrderInLayer;

    // Start is called before the first frame update
    void Start()
    {
        if (line == null)
            line = this.GetComponent<LineRenderer>();

        Debug.Log("laser creating..");
        // line = gameObject.GetComponent<LineRenderer>();
        line.sortingLayerName = SortingLayer;
        line.sortingOrder = OrderInLayer;
        line.sortingOrder = 4;
        line.enabled = false;
        
        // hide mouse cursor
        // don't hide..
    }

    // Update is called once per frame
    void Update()
    {
        if (line == null)
            line = this.GetComponent<LineRenderer>();
        line.sortingOrder = 1;

        if (Input.GetButtonDown("Fire1"))
        {
            Debug.Log("key down");
            // stop and restart just in case of
            //StopCoroutine("FireLaser");
            StartCoroutine("FireLaser");
        }
    }

    private double startTime;

    IEnumerator FireLaser()
    {
        Debug.Log("start FireLaser");
        line.enabled = true;

        // 5초간 레이져 발사
        startTime = Time.time;

        while (Input.GetButton("Fire1"))
        //while (startTime < 5.0)
        {
            Debug.Log("transform.position: " + transform.position);
            Ray ray = new Ray(new Vector3Int(0, 40, 1), Vector3.right);
            // RaycastHit2D hit; // if the ray hits 

            // RaycastHit2D hit = Physics2D.Raycast(ray.origin, Vector2.right, 4000);
            Debug.Log(" - - - - - - - - - -");
            Debug.Log("ray.origin: " + ray.origin);
            Debug.Log("ray.GetPoint(1000): " + ray.GetPoint(1000));
            line.SetPosition(0, ray.origin);
            line.SetPosition(1, ray.GetPoint(1000));

            Debug.Log("----------------------------------------------------------------------");
            /*if (hit.collider)
                line.SetPosition(1, hit.point);
            else
                line.SetPosition(1, ray.GetPoint(4000));*/
            yield return null;
        }

        // Debug.Log("ended 5 seconds");
        line.enabled = false;
    }
}
