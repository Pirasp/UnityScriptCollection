using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
this class creates a laser beam between a start and end point by rescaling a prefab
*/

public class Laser 
{
    public static void ShowLaser (Vector3 originPoint, Vector3 hitPoint, GameObject laserPrefab)
    {
        laserPrefab.SetActive(true);

        laserPrefab.transform.position = Vector3.Lerp(originPoint, hitPoint, .5f);
        
        laserPrefab.transform.LookAt(hitPoint);
        
        laserPrefab.transform.localScale = new Vector3( laserPrefab.transform.localScale.x, 
                                                        laserPrefab.transform.localScale.y, 
                                                     Vector3.Distance(originPoint, hitPoint));
    }
}
