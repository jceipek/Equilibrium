using UnityEngine;
using System.Collections;

public class Connection : MonoBehaviour {

    private Node m_start_node = null;
    private Node m_end_node = null;
    private Vector3 m_end_point; // If the connection is incomplete

    /*
    void OnDrawGizmos () {
        if (m_start_node) {
            Gizmos.DrawLine(m_start_node.transform.position, m_end_point);
        }
    }
    */

    public void FinishConnectionWithEndNode (Node end_node) {
        m_end_node = end_node;
        m_start_node.AddConnection(this); // TODO (Julian): Should this instead go in InitializeWithStartNode?
        m_end_node.AddConnection(this);
    }

    public void InitializeWithStartNode (Node start_node) {
        m_start_node = start_node;
        m_end_point = start_node.gameObject.transform.position;
    }

    public bool CanFinishConnectionWithEndNode (Node end_node) {
        // TODO: Give this a length requirement.
        return true;
    }

    public void DestroyConnection () {
        if (m_start_node) {
            m_start_node.RemoveConnection(this);
        }
        if (m_end_node) m_end_node.RemoveConnection(this);
        Destroy(gameObject);
    }

    public Node GetStartNode () {
        return m_start_node;
    }

    public Node GetEndNode () {
        return m_end_node;
    }

    public Vector3 GetEndPoint () {
        return m_end_point;
    }

    public bool IsStarted () {
        return (m_start_node != null);
    }

    public bool IsComplete () {
        return (IsStarted() && m_end_node != null);
    }

    private float GetLength () {
        if (IsComplete()) {
            return (m_start_node.transform.position - m_end_node.transform.position).magnitude;
        } else if (IsStarted()) {
            return (m_start_node.transform.position - m_end_point).magnitude;
        } else {
            return 0.0f;
        }
    }
}
