using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TrailSpawner : MonoBehaviour {

    public Node m_node;
    public InfectedNodeController m_infection;
    public float m_velocity;
    public float m_refresh_frequency;
    public Color m_normal_color;
    public Color m_infected_color;

    // Use this for initialization
    void Start () {
        StartCoroutine(SpawnTimer());
    }

    IEnumerator SpawnTimer () {
        yield return new WaitForSeconds(m_refresh_frequency);
        Spawn();
        StartCoroutine(SpawnTimer());
    }

    private void Spawn () {
        foreach (Connection connection in m_node.m_connections) {
            Node target_node;
            Node start_node = connection.GetStartNode();
            Node end_node = connection.GetEndNode();
            if (m_node != start_node) {
                target_node = start_node;
            } else {
                target_node = end_node;
            }
            Vector3 spawn_location = gameObject.transform.position + Random.insideUnitSphere * 0.5f;
            Vector3 position_difference = (target_node.transform.position - m_node.transform.position);
            float distance = position_difference.magnitude;
            GameObject trail = Instantiate(Resources.Load("Trail"), spawn_location, gameObject.transform.rotation) as GameObject;
            TrailController trail_controller = trail.GetComponent<TrailController>();
            float life;
            if (m_velocity <= 0.0f) {
                life = 0.0f;
            } else {
                life = distance/m_velocity;
            }
            Vector3 velocity = position_difference.normalized * m_velocity;
            if (m_infection.enabled) {
                trail_controller.InitializeWith(m_infected_color, velocity, life);
            } else {
                trail_controller.InitializeWith(m_normal_color, velocity, life);
            }
        }
    }
}