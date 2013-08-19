using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NodeVisualizer : MonoBehaviour
{
    private Node m_Node;
    private Infected m_Infection;

    void Start ()
    {
        UpdateInfection();
    }

    public void InitializeWithNode (Node node)
    {
        m_Node = node;
        m_Infection = m_Node.GetComponentInChildren<Infected>();
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