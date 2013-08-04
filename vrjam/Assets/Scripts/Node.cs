using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Node : MonoBehaviour {

    // Note: Will be made private
    public List<Connection> m_connections;


    void Awake () {
        if (m_connections != null) m_connections = new List<Connection>();
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

    public void GainMass (float mass) {
        Mass mass_component = gameObject.GetComponent<Mass>();
        if (mass_component) {
            mass_component.TryToIncreaseBy(mass);
        } else {
            Debug.Log("Has no mass component!");
        }
    }

    public void LoseMass (float mass) {
        Mass mass_component = gameObject.GetComponent<Mass>();
        if (mass_component) {
            mass_component.TryToDecreaseBy(mass);
        } else {
            Debug.Log("Has no mass component!");
        }
    }

}