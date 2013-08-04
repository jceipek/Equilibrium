using UnityEngine;
using System.Collections;

public class Connection : MonoBehaviour {

    // Note: Will be made private
    public Node m_start_node = null;
    public Node m_end_node = null;
    public Vector3 m_end_point; // If the connection is incomplete

    public void InitializeWithStartNode (Node start_node) {
        m_start_node = start_node;
        m_end_point = start_node.gameObject.transform.position;
    }

    public void FinishConnectionWithEndNode (Node end_node) {
        m_end_node = end_node;
        m_start_node.AddConnection(this); // TODO (Julian): Should this instead go in InitializeWithStartNode?
        m_end_node.AddConnection(this);
    }

    public void DestroyConnection () {
        if (m_start_node) m_start_node.RemoveConnection(this);
        if (m_end_node) m_end_node.RemoveConnection(this);
        Destroy(gameObject);
    }

    public bool GetStartNode () {
        return m_start_node;
    }

    public bool IsStarted () {
        return (m_start_node != null);
    }

    public bool IsComplete () {
        return (IsStarted() && m_end_node != null);
    }

    void Update () {
        if (IsComplete()) {
            Mass start_node_mass = m_start_node.GetComponent<Mass>();
            Mass end_node_mass = m_end_node.GetComponent<Mass>();
            float delta = RulesManager.g.m_TRANSFER_SPEED * Time.deltaTime;
            if (Mathf.Abs(start_node_mass.Get() - end_node_mass.Get()) < delta) {
                delta = Mathf.Abs(start_node_mass.Get() - end_node_mass.Get())/2.0f;
            }

            if (start_node_mass.Get() > end_node_mass.Get()) {
                end_node_mass.IncreaseBy(delta);
                start_node_mass.DecreaseBy(delta);
            } else if (start_node_mass.Get() < end_node_mass.Get()) {
                end_node_mass.DecreaseBy(delta);
                start_node_mass.IncreaseBy(delta);
            }
        }
    }

    void OnDrawGizmos () {
        if (m_start_node) {
            if (m_end_node) {
                Gizmos.DrawLine(m_start_node.gameObject.transform.position,
                                m_end_node.gameObject.transform.position);
            } else {
                Gizmos.DrawLine(m_start_node.gameObject.transform.position,
                                m_end_point);
            }
        }

    }
}
