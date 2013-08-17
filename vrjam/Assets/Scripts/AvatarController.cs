using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum enINPUT_MODE {KeyToMove, WaitToMove};

public class AvatarController : MonoBehaviour {

    public enINPUT_MODE m_input_mode = enINPUT_MODE.WaitToMove;

    public Camera m_camera_target; // TODO (JULIAN): Put this functionality elsewhere

    public Node m_next_node;
    public float m_speed;
    private bool m_is_moving = false;

    private Node m_current_node;

    void Start () {
        if (!m_next_node) {
            StartCoroutine(SetupRandom());
        }
        m_is_moving = true;
    }

    void Update () {
        if (m_next_node) {
            if (m_is_moving) {
                Vector3 position_difference = m_next_node.transform.position - gameObject.transform.position;
                if (position_difference.magnitude < 0.1f) {
                    m_is_moving = false;
                }
                Vector3 direction = position_difference.normalized;
                gameObject.transform.position += (direction * m_speed);
            } else {
                m_current_node = m_next_node;
                if (m_input_mode == enINPUT_MODE.KeyToMove) {
                    if (Input.GetButtonDown("Select")) {
                        PickNodeInSight();
                        m_is_moving = true;
                    }
                } else if (m_input_mode == enINPUT_MODE.WaitToMove) {
                    StartCoroutine(MoveInSeconds(0.3f));
                }
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
        if (node_objects.Length > 0) {
            m_next_node = node_objects[0];
        }
    }

    private void PickNodeInSight () {
        float largest_dot = -0.0f;
        List<Node> candidate_nodes = m_current_node.GetConnectedNodes();
        foreach (Node node in candidate_nodes) {
            Vector3 node_direction = (node.transform.position - m_current_node.transform.position).normalized;
            float current_dot = Vector3.Dot(node_direction, m_camera_target.transform.forward);
            if (current_dot > largest_dot) {
                largest_dot = current_dot;
                m_next_node = node;
            }
        }
    }

    /*void OnDrawGizmos () {
        Gizmos.color = Color.blue;
        Vector3 position = m_camera_target.transform.position;
        Vector3 end = position + m_camera_target.transform.forward * 10.0f;
        Gizmos.DrawLine(position, end);
    }*/

}
