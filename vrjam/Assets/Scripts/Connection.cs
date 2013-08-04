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
            StepUpdateMassBalance(start_node_mass, end_node_mass, RulesManager.g.m_TRANSFER_SPEED);
        } else if (IsStarted()) {
            Mass start_node_mass = m_start_node.GetComponent<Mass>();
            SiphonFromMassToReachMinimum(start_node_mass, m_mass, RulesManager.g.m_TRANSFER_SPEED);
        }
    }

    // Siphons from a to b or vice versa unless they have equal free mass
    public void StepUpdateMassBalance (Mass mass_a, Mass mass_b, float transfer_rate) {
        float free_mass_difference = Mathf.Abs(mass_a.GetAmountAvailable() - mass_b.GetAmountAvailable());
        float delta = transfer_rate * Time.deltaTime;
        if (free_mass_difference < delta) {
            delta = free_mass_difference/2.0f;
        }

        if (mass_a.GetAmountAvailable() > mass_b.GetAmountAvailable()) {
            mass_a.TryToDecreaseBy(delta);
            mass_b.TryToIncreaseBy(delta);
        } else if (mass_b.GetAmountAvailable() > mass_a.GetAmountAvailable()) {
            mass_b.TryToDecreaseBy(delta);
            mass_a.TryToIncreaseBy(delta);
        }
    }

    // Siphons from mass_large to mass_probably_too_small
    public void SiphonFromMassToReachMinimum (Mass mass_large, Mass mass_probably_too_small, float transfer_rate) {
        if (mass_probably_too_small.GetAmountAvailable() < 0) {
            float mass_needed = -mass_probably_too_small.GetAmountAvailable();
            float free_mass = mass_large.GetAmountAvailable();
            float delta = transfer_rate * Time.deltaTime;
            if (mass_needed < delta) {
                delta = mass_needed;
            }
            if (mass_large.TryToDecreaseBy(delta)) {
                mass_probably_too_small.TryToIncreaseBy(delta);
            } // Otherwise, there is no way to supply the necessary mass
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
        float minimum = DetermineMassNeededForConnectionBetween(m_start_node, m_end_node);
        m_mass.InitializeMinimum(minimum);
        return true; // Change to return false when would be unable to reach
    }

    public void DestroyConnection () {
        if (m_start_node) {
            m_start_node.GainMass(m_mass.Get());
            m_mass.TryToDecreaseBy(m_mass.Get());
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

    // TODO (Julian): Replace this with a function that tries to
    // get to the end point (once connections need mass for length)
    //public void SetEndPoint (Vector3 end_point) {
    //    m_end_point = end_point;
    //}

    public void TryToReachPoint (Vector3 point) {
        float mass_required_to_reach = DetermineMassNeededForConnectionBetween(m_start_node.transform.position, m_end_point);
        float mass_available = m_start_node.GetFreeMass();
        if (mass_required_to_reach <= mass_available) {
            m_end_point = point;
            m_mass.InitializeMinimum(mass_required_to_reach);
        } else {
            Vector3 direction = (point - m_start_node.transform.position).normalized;
            m_end_point = ProjectMassAlongDirectionFromPoint (source_point: m_start_node.transform.position,
                                                              direction: direction,
                                                              mass: mass_available);
            m_mass.InitializeMinimum(mass_available);
        }
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

    private Vector3 ProjectMassAlongDirectionFromPoint (Vector3 source_point, Vector3 direction, float mass) {
        return direction * RulesManager.g.m_MASS_TO_LENGTH_RATIO + source_point;
    }

    private float DetermineMassNeededForConnectionBetween (Node start_node, Node end_node) {
        return DetermineMassNeededForConnectionBetween(start_node.transform.position,
                                                     end_node.transform.position);
    }

    private float DetermineMassNeededForConnectionBetween (Vector3 start_position, Vector3 end_position) {
        return (start_position - end_position).magnitude * RulesManager.g.m_MASS_TO_LENGTH_RATIO;
    }
}
