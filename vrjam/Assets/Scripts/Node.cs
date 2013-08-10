using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Node : MonoBehaviour {

    // Note: Will be made private
    public List<Connection> m_connections;

    private Highlighter m_visuals;

    void Awake () {
        if (m_connections != null) m_connections = new List<Connection>();
        m_visuals = gameObject.GetComponentInChildren<Highlighter>();
    }

    public void AddConnection (Connection connection) {
        m_connections.Add(connection);
    }

    public void RemoveConnection (Connection connection) {
        m_connections.Remove(connection);
    }

    public bool HasConnection (Connection connection) {
        return m_connections.Contains(connection);
    }

    public bool DoesShareConnectionWith (Node other_node) {
        foreach (Connection connection in m_connections) {
            if (other_node.HasConnection(connection)) return true;
        }
        return false;
    }

    public Connection ConnectionSharedWith (Node other_node) {
        foreach (Connection connection in m_connections) {
            Node start_node = connection.GetStartNode();
            Node end_node = connection.GetEndNode();
            if (start_node == other_node || end_node == other_node) {
                return connection;
            }
        }
        return null;
    }

    public List<Node> GetConnectedNodes () {
        List<Node> connected = new List<Node>();
        foreach (Connection connection in m_connections) {
            Node start_node = connection.GetStartNode();
            if (start_node != this) {
                connected.Add(start_node);
            } else {
                Node end_node = connection.GetEndNode();
                connected.Add(end_node);
            }
        }
        return connected;
    }

    public void Highlight () {
        m_visuals.Highlight();
    }

    public void UnHighlight () {
        m_visuals.UnHighlight();
    }

}