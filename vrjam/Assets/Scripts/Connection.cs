using UnityEngine;
using System.Collections;

[RequireComponent (typeof (Mass))]
public class Connection : MonoBehaviour {

    private Node m_start_node = null;
    private Node m_end_node = null;
    private Vector3 m_end_point; // If the connection is incomplete
    private Mass m_mass;

    void Awake () {
        m_mass = this.GetComponent<Mass>();
    }

    void Update () {
        // TODO XXX (Julian): This is currently skipping mass in the connection itself
        if (IsComplete()) {
            Mass start_node_mass = m_start_node.GetComponent<Mass>();
            Mass end_node_mass = m_end_node.GetComponent<Mass>();
            float delta = RulesManager.g.m_TRANSFER_SPEED * Time.deltaTime;
            if (Mathf.Abs(start_node_mass.Get() - end_node_mass.Get()) < delta) {
                delta = Mathf.Abs(start_node_mass.Get() - end_node_mass.Get())/2.0f;
            }

            if (start_node_mass.Get() > end_node_mass.Get()) {
                end_node_mass.TryToIncreaseBy(delta);
                start_node_mass.TryToDecreaseBy(delta);
            } else if (start_node_mass.Get() < end_node_mass.Get()) {
                end_node_mass.TryToDecreaseBy(delta);
                start_node_mass.TryToIncreaseBy(delta);
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

    public void InitializeWithStartNode (Node start_node) {
        m_start_node = start_node;
        m_end_point = start_node.gameObject.transform.position;
    }

    public bool TryToFinishConnectionWithEndNode (Node end_node) {
        m_end_node = end_node;
        m_start_node.AddConnection(this); // TODO (Julian): Should this instead go in InitializeWithStartNode?
        m_end_node.AddConnection(this);
        return true; // Change to return false when would be unable to reach
    }

    public void DestroyConnection () {
        if (m_start_node) m_start_node.RemoveConnection(this);
        if (m_end_node) m_end_node.RemoveConnection(this);
        Destroy(gameObject);
    }

    public bool GetStartNode () {
        return m_start_node;
    }

    public bool GetEndNode () {
        return m_end_node;
    }

    public bool IsStarted () {
        return (m_start_node != null);
    }

    public bool IsComplete () {
        return (IsStarted() && m_end_node != null);
    }

    private void ComputeMinimumMass () {
        float minimum = (m_start_node.transform.position - m_end_node.transform.position).magnitude;
        minimum *= RulesManager.g.m_MASS_TO_LENGTH_RATIO;
        m_mass.InitializeMinimum(minimum);
    }
}
