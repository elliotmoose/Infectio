using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserNew : MonoBehaviour
{
    protected WeaponData _weaponData;
    protected GameObject _owner;

    LineRenderer lineRenderer = null;
    Vector3[] startingLineRendererPoints = null;

    //public Transform laserSpawnPoint;

    // Start is called before the first frame update
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();

        //set the max points in the line renderer.
        lineRenderer.positionCount = 4;

        //setup the array to store the starting positions of the line renderer points.
        startingLineRendererPoints = new Vector3[4];

        lineRenderer.SetPosition(0, transform.Find("Pos1").localPosition);
        lineRenderer.SetPosition(1, transform.Find("Pos2").localPosition);
        lineRenderer.SetPosition(2, transform.Find("Pos3").localPosition);
        lineRenderer.SetPosition(3, transform.Find("Pos4").localPosition);

        lineRenderer.GetPositions(startingLineRendererPoints);

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        bool hitSomething = false;

        if (lineRenderer)
        {
            RaycastHit hitInfo;

            // create an array to hold the line renderer points
            Vector3[] newPointsInLine = null;

            for (int i = 0; i < startingLineRendererPoints.Length - 1; i++)
            {
                if (Physics.Linecast(startingLineRendererPoints[i], startingLineRendererPoints[i + 1], out hitInfo))
                {
                    Debug.Log("Line cast between " + i + " " + startingLineRendererPoints[i] + " and " + i + 1 + " " + startingLineRendererPoints[i + 1]);

                    //initialize the new array to the furthest point + 1 since the array is 0-based
                    newPointsInLine = new Vector3[(i + 1) + 1];

                    //transfer the points we need to the new array
                    for (int i2 = 0; i2 < newPointsInLine.Length; i2++)
                    {
                        newPointsInLine[i2] = startingLineRendererPoints[i2];
                    }
                    
                    //set the current point to the raycast hit point (the end of the line renderer)
                    newPointsInLine[i + 1] = hitInfo.point;

                    //flag that we hit something
                    hitSomething = true;

                    break;
                }
            }

            if (hitSomething)
            {
                //use new points for the line renderer
                lineRenderer.positionCount = newPointsInLine.Length;

                lineRenderer.SetPositions(newPointsInLine);

                // attack the enemy that it hits
            }
            else
            {
                //use old points for the line renderer
                lineRenderer.positionCount = startingLineRendererPoints.Length;

                lineRenderer.SetPositions(startingLineRendererPoints);
            }
        }
    }

    public void Activate(WeaponData weaponData, GameObject owner)
    {
        this._weaponData = weaponData;
        this._owner = owner;
    }
}
