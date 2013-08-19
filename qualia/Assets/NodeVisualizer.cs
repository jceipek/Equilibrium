using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NodeVisualizer : MonoBehaviour
{
    public Node m_Node;
    private Infected m_Infection;

    void OnEnable ()
    {
        m_Infection = m_Node.GetComponentInChildren<Infected>();
    }

    void Start ()
    {
        UpdateInfection();
    }

    public void UpdateInfection ()
    {
        if (m_Infection.enabled)
        {
            renderer.material.color = Constants.g.NODE_INFECTED_COLOR;
        }
        else
        {
            renderer.material.color = Constants.g.NODE_NORMAL_COLOR;
        }
    }

    public void Update ()
    {
        UpdateInfection();
    }
}