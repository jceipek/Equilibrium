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
        // TODO (JULIAN): Seriously Refactor this
        // TODO XXX (Julian): This is currently skipping mass in the connection itself
        if (IsComplete()) {
            Mass start_node_mass = m_start_node.GetComponent<Mass>();
            Mass end_node_mass = m_end_node.GetComponent<Mass>();
            float delta = RulesManager.g.m_TRANSFER_SPEED * Time.deltaTime;
            float mass_difference = Mathf.Abs(start_node_mass.Get() - end_node_mass.Get());
            if (mass_difference < delta) {
                delta = mass_difference/2.0f;
            }

            if (start_node_mass.Get() > end_node_mass.Get()) {
                end_node_mass.TryToIncreaseBy(delta);
                start_node_mass.TryToDecreaseBy(delta);
            } else if (start_node_mass.Get() < end_node_mass.Get()) {
                end_node_mass.TryToDecreaseBy(delta);
                start_node_mass.TryToIncreaseBy(delta);
            }
        } else if (IsStarted()) {
            // Siphon mass required for endpoint pos
            Mass start_node_mass = m_start_node.GetComponent<Mass>();
            float required_mass = DetermineMassNeededForConnectionBetween(m_start_node.transform.position, m_end_point);
            float mass_difference = Mathf.Abs(start_node_mass.Get() - m_mass.Get());
            if (required_mass > m_mass.Get()) {
                float delta = RulesManager.g.m_TRANSFER_SPEED * Time.deltaTime;
                if (mass_difference < delta) {
                    delta = mass_difference/2.0f;
                }

                // Steal Mass from start node (will now die because too much mass needed?)
                if (start_node_mass.Get() > m_mass.Get()) {
                    m_mass.TryToIncreaseBy(delta);
                    start_node_mass.TryToDecreaseBy(delta);
                } else if (start_node_mass.Get() < m_mass.Get()) {
                    m_mass.TryToDecreaseBy(delta);
                    start_node_mass.TryToIncreaseBy(delta);
                }
            } else if (required_mass < m_mass.Get()) {
                float delta = RulesManager.g.m_TRANSFER_SPEED * Time.deltaTime;
                if (mass_difference < delta) {
                    delta = mass_difference/2.0f;
                }

                start_node_mass.TryToIncreaseBy(delta);
                m_mass.TryToDecreaseBy(delta);
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

    public Node GetStartNode () {
        return m_start_node;
    }

    public Node GetEndNode () {
        return m_end_node;
    }

    // TODO (Julian): Replace this with a function that tries to
    // get to the end point (once connections need mass for length)
    public void SetEndPoint (Vector3 end_point) {
        m_end_point = end_point;
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

    private float DetermineMassNeededForConnectionBetween (Node start_node, Node end_node) {
        return DetermineMassNeededForConnectionBetween(start_node.transform.position,
                                                     end_node.transform.position);
    }

    private float DetermineMassNeededForConnectionBetween (Vector3 start_position, Vector3 end_position) {
        return (start_position - end_position).magnitude * RulesManager.g.m_MASS_TO_LENGTH_RATIO;
    }

    private void ComputeMinimumMass () {
        float minimum = DetermineMassNeededForConnectionBetween(m_start_node, m_end_node);
        m_mass.InitializeMinimum(minimum);
    }
}
