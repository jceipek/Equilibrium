using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AvatarController : MonoBehaviour {

    public Camera m_camera_target; // TODO (JULIAN): Put this functionality elsewhere

    public Node m_current_node;
    public Node m_next_node;
    public float m_speed;
    private bool m_is_moving = false;

    void Start () {
        StartCoroutine(SetupRandom()); // TODO (JULIAN): Replace with manual placement
    }

    void Update () {
        if (m_next_node) {
            if (m_is_moving) {
                Vector3 position_difference = m_next_node.transform.position - gameObject.transform.position;
                if (position_difference.magnitude < 0.5f) {
                    m_is_moving = false;
                }
                Vector3 direction = position_difference.normalized;
                gameObject.transform.position += (direction * m_speed);
            } else {
                if (Input.GetButtonDown("Select")) {
                    RandomlyPickNextNode(); // TODO: Change to actual selection behavior
                    m_is_moving = true;
                }
            }
        }
    }

    private IEnumerator SetupRandom () {
        // Wait until level has been randomly set up:
        yield return new WaitForSeconds(0.5f);
        Node[] node_objects = FindObjectsOfType(typeof(Node)) as Node[];
        m_next_node = node_objects[0];
        m_is_moving = true;
    }

    private void RandomlyPickNextNode () {
        float smallest_angle = Mathf.Infinity;
        List<Node> candidate_nodes = m_next_node.GetConnectedNodes();
        foreach (Node node in candidate_nodes) {
            Vector3 node_direction = node.transform.position - m_next_node.transform.position;
            Quaternion rotation = Quaternion.FromToRotation(m_camera_target.transform.forward, node_direction);
            float current_angle = Mathf.Abs(Quaternion.Angle(m_camera_target.transform.rotation, rotation));
            Debug.Log(current_angle);
            if (current_angle < smallest_angle) {
                smallest_angle = current_angle;
                m_next_node = node;
            }
        }
        //if (candidate_nodes.Count > 0) {
        //    m_next_node = candidate_nodes[0];
        //}
    }

    void OnDrawGizmos () {
        Vector3 position = m_camera_target.transform.position;
        Vector3 end = position + m_camera_target.transform.forward * 3.0f;
        Gizmos.DrawLine(position, end);
    }

}
