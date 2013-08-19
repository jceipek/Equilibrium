using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class Constants : MonoBehaviour
{

    public static Constants g;

    public float INFECTION_SPREAD_SPEED = 1.8f;
    public Color CONNECTION_NORMAL_COLOR;
    public Color CONNECTION_INFECTED_COLOR;
    public Color NODE_NORMAL_COLOR;
    public Color NODE_INFECTED_COLOR;

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