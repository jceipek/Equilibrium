using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RandomSphereGenerator : MonoBehaviour {

    public int m_node_count;
    public float m_sphere_radius;
    public float m_connection_count;

    // Use this for initialization
    void Start () {
        Generate();
    }

    private Node GenerateNodeAt (Vector3 pos) {
        GameObject node = (GameObject)Instantiate(Resources.Load("Node"));
        node.transform.position = pos;
        Mass mass_component = node.GetComponent<Mass>();
        mass_component.Initialize(Random.value * 2.0f + 0.5f);
        return node.GetComponent<Node>();
    }

    private void Generate () {
        List<Node> node_list = new List<Node>();
        for (int index = 0; index < m_node_count; index++) {
            //Vector3 random_vector = Random.onUnitSphere * m_sphere_radius;
            Vector3 random_vector = Random.insideUnitSphere * m_sphere_radius;
            Node node = GenerateNodeAt(random_vector);
            node_list.Add(node);
        }

        int max_connection_count = m_node_count*(m_node_count - 1) / 2;
        if (m_connection_count > max_connection_count) {
            m_connection_count = max_connection_count; // So we don't have an infinite loop!
        }
        bool success = false;
        for (int index = 0; index < m_connection_count; index++) {
            do {
                int first = index % m_node_count;
                int second = Random.Range(0, node_list.Count);
                Node first_node = node_list[first];
                Node second_node = node_list[second];
                success = (first != second && !first_node.DoesShareConnectionWith(second_node));
                if (success) {
                    GameObject connection_object = (GameObject)Instantiate(Resources.Load("Connection"));
                    Connection connection = connection_object.GetComponent<Connection>();
                    connection.InitializeWithStartNode(first_node);
                    connection.TryToFinishConnectionWithEndNode(second_node); // TODO: Change once this doesn't work anymore by giving the connection extra internal mass
                }
            } while (!success);
        }
    }
}
