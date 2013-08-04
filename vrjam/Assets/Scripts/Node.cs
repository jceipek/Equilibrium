using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Node : MonoBehaviour {

    // Note: Will be made private
    public List<Connection> m_connections;

    public void AddConnection (Connection connection) {
        m_connections.Add(connection);
    }

    public void RemoveConnection (Connection connection) {
        m_connections.Remove(connection);
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

    void Awake () {
        if (m_connections != null) m_connections = new List<Connection>();
    }
}