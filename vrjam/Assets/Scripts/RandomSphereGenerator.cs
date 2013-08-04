using UnityEngine;
using System.Collections;

public class RandomSphereGenerator : MonoBehaviour {

    public int m_node_count;
    public float m_sphere_radius;

    // Use this for initialization
    void Start () {
        Generate();
    }

    private void GenerateNodeAt (Vector3 pos) {
        GameObject node = (GameObject)Instantiate(Resources.Load("Node"));
        node.transform.position = pos;
        Mass mass_component = node.GetComponent<Mass>();
        mass_component.m_mass = Random.value * 2.0f;
    }

    private void Generate () {
        for (int index = 0; index < m_node_count; index++) {
            //Vector3 random_vector = Random.onUnitSphere * m_sphere_radius;
            Vector3 random_vector = Random.insideUnitSphere * m_sphere_radius;
            GenerateNodeAt(random_vector);
        }
    }
}
