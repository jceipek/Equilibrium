using UnityEngine;
using System.Collections;

public class FollowNode : MonoBehaviour {

    public GameObject m_collider;
    public float speed;
    public Node m_current_node;
    public Node m_next_node;
    public Connection m_current_connection;
    private bool m_genned = false;

    void Start () {
        StartCoroutine(SetupRandom());

    }

    void Update () {
        if (m_genned) {
            if ((m_next_node.transform.position - gameObject.transform.position).magnitude < 0.5f) {
                GotoNextNode();
            }
            Vector3 direction = (m_next_node.transform.position - gameObject.transform.position).normalized;
            gameObject.transform.position += (direction * speed);
        }
    }

    IEnumerator SetupRandom () {
        yield return new WaitForSeconds(0.5f);
        Node[] node_objects = FindObjectsOfType(typeof(Node)) as Node[];
        if (node_objects[0].m_connections.Count > 0) {
            m_current_node = node_objects[0];
            Node start_node = node_objects[0].m_connections[0].GetStartNode();
            Node end_node = node_objects[0].m_connections[0].GetEndNode();
            m_current_connection = node_objects[0].m_connections[0];
            if (m_current_node != start_node) {
                m_next_node = start_node;
            } else {
                m_next_node = end_node;
            }
        } else {
            Debug.Log("OOPS");
        }
        m_genned = true;
    }

    public void GotoNextNode () {
        int num = 0;
        Connection new_connection = null;
        Node next_node = m_current_node;
        if (m_next_node.m_connections.Count > 1) {
            num = Random.Range(0, m_next_node.m_connections.Count);
            foreach (Connection c in m_next_node.m_connections) {
                if(c.GetInstanceID() != m_current_connection.GetInstanceID()) {
                    new_connection = c;
                    Debug.Log("YAY");
                    break;
                }
            }
        }
        if (m_next_node.m_connections.Count > 0) {
            new_connection = m_next_node.m_connections[num];

            Node start_node = new_connection.GetStartNode();
            Node end_node = new_connection.GetEndNode();
            if (start_node == next_node) {
                next_node = end_node;
            } else {
                next_node = start_node;
            }
        }
        m_current_node = m_next_node;
        m_next_node = next_node;
        m_current_connection = new_connection;
    }
}
