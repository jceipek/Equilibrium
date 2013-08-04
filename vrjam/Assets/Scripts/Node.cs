using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Node : MonoBehaviour {

    public List<Connection> m_connections;

    void Awake () {
        if (m_connections != null) m_connections = new List<Connection>();
    }
}