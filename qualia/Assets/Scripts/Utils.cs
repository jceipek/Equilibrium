using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class Utils
{
    public static bool AreGameObjectsColliding (GameObject a, GameObject b)
    {
        Vector3 positionDifference = a.transform.position - b.transform.position;
        return IsDistanceInCollideRange(positionDifference.magnitude);
    }

    public static bool IsDistanceInCollideRange (float distance)
    {
        return distance < Constants.g.COLLISION_DISTANCE;
    }
}