using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RandomSphereGenerator : MonoBehaviour {

    public int m_node_count;
    public float m_node_min_mass;
    public float m_node_max_mass;
    public int m_connection_count;

    public float m_sphere_radius;
    public bool m_allow_nodes_inside_sphere;

    private List<Node> m_node_list = new List<Node>();

    // Use this for initialization
    void Start () {
        Generate();
    }

    private Node GenerateNodeAt (Vector3 pos) {
        GameObject node = Instantiate(Resources.Load("Node")) as GameObject;
        node.transform.position = pos;
        Mass mass_component = node.GetComponent<Mass>();
        mass_component.Initialize(Random.value * (m_node_max_mass - m_node_min_mass) + m_node_min_mass);
        return node.GetComponent<Node>();
    }

    private void Generate () {
        GenerateNodes();
        GenerateConnections();
    }

    private void GenerateNodes () {
        for (int index = 0; index < m_node_count; index++) {
            Vector3 random_vector;
            if (m_allow_nodes_inside_sphere) {
                random_vector = Random.insideUnitSphere * m_sphere_radius;
            } else {
                random_vector = Random.onUnitSphere * m_sphere_radius;
            }

            Node node = GenerateNodeAt(random_vector);
            m_node_list.Add(node);
        }
    }

    private void GenerateConnections () {
        EnsureConnectionCountIsPossible();
        for (int index = 0; index < m_connection_count; index++) {
            bool success = false;
            do {
                int first = index % m_node_count;
                int second = Random.Range(0, m_node_list.Count);
                Node first_node = m_node_list[first];
                Node second_node = m_node_list[second];
                success = (first != second && !first_node.DoesShareConnectionWith(second_node));
                if (success) {
                    GameObject connection_object = (GameObject)Instantiate(Resources.Load("Connection"));
                    Connection connection = connection_object.GetComponent<Connection>();
                    connection.InitializeWithStartNode(first_node);
                    connection.MassCreateAndFinishConnectionWithEndNode(second_node); // TODO: Change once this doesn't work anymore by giving the connection extra internal mass
                }
            } while (!success);
        }
    }

    private void EnsureConnectionCountIsPossible () {
        int max_connection_count = m_node_count*(m_node_count - 1) / 2;
        if (m_connection_count > max_connection_count) {
            m_connection_count = max_connection_count; // So we don't have an infinite loop!
        }
    }
}
