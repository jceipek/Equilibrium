using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class Constants : MonoBehaviour
{

    public static Constants g;

    public float COLLISION_DISTANCE = 0.1f;

    public float INFECTION_SPREAD_SPEED = 1.8f;

    public Color CONNECTION_NORMAL_COLOR;
    public Color CONNECTION_INFECTED_COLOR;

    public Color NODE_NORMAL_COLOR;
    public Color NODE_INFECTED_COLOR;

    public Color TRAIL_NORMAL_TINT_COLOR;
    public Color TRAIL_INFECTED_TINT_COLOR;

    void OnEnable ()
    {
        if (!g)
        {
            g = this;
        }
        else
        {
            throw new Exception("Only one instance of Constants can exist!");
        }
    }
}