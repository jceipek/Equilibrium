using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Node : MonoBehaviour
{
    // Not used during gameplay (For setup in editor)
    public List<Node> m_NodesForWhichToMakeConnections;

    private Dictionary<Node, Connection> m_ConnectedNodesToConnections = new Dictionary<Node, Connection>();
    private NodeVisualizer m_Visualizer;
    private TrailEmitter m_Emitter;
    //public List<Node> m_DebugNodes;

    void OnEnable ()
    {
        m_Visualizer = GetComponentInChildren<NodeVisualizer>();
        m_Visualizer.InitializeWithNode(this);

        m_Emitter = GetComponentInChildren<TrailEmitter>();
        m_Emitter.InitializeWithNode(this);

        foreach (Node node in m_NodesForWhichToMakeConnections)
        {
            AddMutualConnectionTo(node);
        }
    }

    void OnDrawGizmos ()
    {
        if (!Application.isPlaying)
        {
            foreach (Node node in m_NodesForWhichToMakeConnections)
            {
                Gizmos.color = Color.white;
                Vector3 offset = (node.transform.position - gameObject.transform.position).normalized/2.0f;
                Gizmos.DrawSphere(gameObject.transform.position + offset, 0.2f);

                Gizmos.color = Color.red;
                Gizmos.DrawLine(node.transform.position, gameObject.transform.position);
            }
        }
    }

    public void AddMutualConnectionTo (Node node)
    {
        if (this == node)
        {
            return;
        }

        Connection connection = GetConnectionWith(node);
        if (connection == null)
        {
            connection = node.GetConnectionWith(this);
        }
        if (connection == null)
        {
            connection = CreateConnectionBetween(this, node);
        }

        m_ConnectedNodesToConnections[node] = connection;
        node.m_ConnectedNodesToConnections[this] = connection;

        /*
        m_DebugNodes = new List<Node>();
        foreach (Node nnode in m_ConnectedNodesToConnections.Keys) {
            m_DebugNodes.Add(nnode);
        }
        */
    }

    public Dictionary<Node, Connection>.KeyCollection GetConnectedNodes ()
    {
        return m_ConnectedNodesToConnections.Keys;
    }

    public bool DoesShareConnectionWith (Node node)
    {
        return m_ConnectedNodesToConnections.ContainsKey(node);
    }

    private Connection CreateConnectionBetween (Node node1, Node node2)
    {
        GameObject connection_object = (GameObject)Instantiate(Resources.Load("Connection"));
        Connection connection = connection_object.GetComponent<Connection>();
        connection.InitializeConnectionBetween(node1, node2);
        return connection;
    }

    public Connection GetConnectionWith (Node node) {
        if (m_ConnectedNodesToConnections.ContainsKey(node))
        {
            return m_ConnectedNodesToConnections[node];
        }
        return null;
    }
}