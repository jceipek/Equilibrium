using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent (typeof (Mass))]
public class Node : MonoBehaviour {

    // Note: Will be made private
    public List<Connection> m_connections;

    private Mass m_mass;
    private Highlighter m_visuals;

    void Awake () {
        if (m_connections != null) m_connections = new List<Connection>();

        m_mass = gameObject.GetComponent<Mass>();
        m_mass.InitializeMinimum(RulesManager.g.m_MINIMUM_NODE_MASS);

        m_visuals = gameObject.GetComponentInChildren<Highlighter>();
    }

    void Update () {
        SphereCollider collider = gameObject.GetComponent<SphereCollider>();
        Mass mass_component = gameObject.GetComponent<Mass>();
        collider.radius = mass_component.Get() * RulesManager.g.m_MASS_TO_SIZE_RATIO / 2.0f;
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

    public float GetFreeMass () {
        return m_mass.GetAmountAvailable();
    }

    public void GainMass (float mass) {
        m_mass.TryToIncreaseBy(mass);
    }

    public void LoseMass (float mass) {
        m_mass.TryToDecreaseBy(mass);
    }

    public void Highlight () {
        m_visuals.Highlight();
    }

    public void UnHighlight () {
        m_visuals.UnHighlight();
    }

}