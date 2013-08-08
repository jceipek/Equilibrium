using UnityEngine;
using System.Collections;

public class TrailSpawner : MonoBehaviour {

	public Node m_node;
	public float m_velocity;

	// Use this for initialization
	void Start () {
		StartCoroutine(Spawn());
	}

	IEnumerator Spawn () {
        yield return new WaitForSeconds(0.2f);
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
        	float distance = (target_node.transform.position - m_node.transform.position).magnitude;
        	GameObject trail = Instantiate(Resources.Load("Trail"), spawn_location, gameObject.transform.rotation) as GameObject;
        	TrailController trail_controller = trail.GetComponent<TrailController>();
        	if (m_velocity <= 0.0f) {
        		trail_controller.m_life = 0.0f;
    		} else {
    			trail_controller.m_life = distance/m_velocity;
    		}
        	trail_controller.m_velocity = (target_node.transform.position - m_node.transform.position).normalized * m_velocity;
        }
        StartCoroutine(Spawn());
    }
}