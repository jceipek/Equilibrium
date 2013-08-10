using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AvatarController : MonoBehaviour {

    public Camera m_camera_target; // TODO (JULIAN): Put this functionality elsewhere

    public Node m_next_node;
    public float m_speed;
    private bool m_is_moving = false;

    private Node m_current_node;

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
                m_current_node = m_next_node;
                StartCoroutine(MoveInSeconds(0.3f));
                //if (Input.GetButtonDown("Select")) {
                //}
            }
        }
    }

    private IEnumerator MoveInSeconds (float seconds) {
        yield return new WaitForSeconds(seconds);
        PickNodeInSight();
        m_is_moving = true;
    }

    private IEnumerator SetupRandom () {
        // Wait until level has been randomly set up:
        yield return new WaitForSeconds(0.5f);
        Node[] node_objects = FindObjectsOfType(typeof(Node)) as Node[];
        m_next_node = node_objects[0];
        m_is_moving = true;
    }

    private void PickNodeInSight () {
        float largest_dot = -0.0f;
        List<Node> candidate_nodes = m_current_node.GetConnectedNodes();
        foreach (Node node in candidate_nodes) {
            Vector3 node_direction = (node.transform.position - m_current_node.transform.position).normalized;
            float current_dot = Vector3.Dot(node_direction, m_camera_target.transform.forward);
            Debug.Log(current_dot);
            if (current_dot > largest_dot) {
                largest_dot = current_dot;
                m_next_node = node;
            }
        }
        //if (candidate_nodes.Count > 0) {
        //    m_next_node = candidate_nodes[0];
        //}
    }

    void OnDrawGizmos () {
        Gizmos.color = Color.blue;
        Vector3 position = m_camera_target.transform.position;
        Vector3 end = position + m_camera_target.transform.forward * 10.0f;
        Gizmos.DrawLine(position, end);
    }

}
