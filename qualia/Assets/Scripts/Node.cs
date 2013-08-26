using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Node : MonoBehaviour
{
    // Not used during gameplay (For setup in editor)
    public List<Node> m_NodesForWhichToMakeConnections;

    private NodeSFX m_SFX;

    private Dictionary<Node, Connection> m_ConnectedNodesToConnections = new Dictionary<Node, Connection>();
    //public List<Node> m_DebugNodes;

    void OnEnable ()
    {
        m_SFX = GetComponentInChildren<NodeSFX>();

        foreach (Node node in m_NodesForWhichToMakeConnections)
        {
            AddMutualConnectionTo(node, audible: false);
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

    public bool AddMutualConnectionTo (Node node, bool audible = true)
    {
        bool madeNewConnection = false;
        if (this == node)
        {
            return false;
        }

        Connection connection = GetConnectionWith(node);
        if (connection == null)
        {
            connection = node.GetConnectionWith(this);
        }
        if (connection == null)
        {
            if (audible)
            {
                m_SFX.PlayConnectionSound(this, node);
            }
            madeNewConnection = true;
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

        return madeNewConnection;
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