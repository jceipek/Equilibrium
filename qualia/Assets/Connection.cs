using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Connection : MonoBehaviour {
	private ConnectionVisualizer m_Visualizer;
    private Node m_StartNode;
    private Node m_EndNode;

    void OnEnable ()
    {
    	m_Visualizer = GetComponentInChildren<ConnectionVisualizer>();
    }

    void OnDrawGizmos ()
    {
        Gizmos.DrawLine(m_StartNode.transform.position, m_EndNode.transform.position);
    }

    public void InitializeConnectionBetween (Node startNode, Node endNode)
    {
        m_StartNode = startNode;
        m_EndNode = endNode;
        m_Visualizer.InitializeWithNodes(m_StartNode, m_EndNode);
    }
}