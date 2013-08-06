using UnityEngine;
using System.Collections;

[RequireComponent (typeof (SelectionController))]
public class ContinuousInputController : MonoBehaviour {

    private SelectionController m_selection_controller;

    private Vector3 m_start_position;
    private Vector3 m_end_position;

    private Ray m_pointer_ray;

    private Node m_selected = null;

    public GameObject m_debug_cursor_REMOVE_ME;

    // Use this for initialization
    void Start () {
        m_selection_controller = gameObject.GetComponent<SelectionController>();
    }

    // Update is called once per frame
    void Update () {
        float mouse_projection_distance = 100.0f;

        bool select_clicked = Input.GetButtonDown("Select");
        bool select_unclicked = Input.GetButtonUp("Select");
        if (select_clicked || select_unclicked) {
            Node pickable_under_cursor = null;
            GameObject under_cursor = FindObjectUnderCursor(cast_distance: mouse_projection_distance, debug: true);
            if (under_cursor) pickable_under_cursor = under_cursor.GetComponent<Node>();

            if (select_clicked && pickable_under_cursor) {
                m_selection_controller.SelectNode(pickable_under_cursor);
                m_selected = pickable_under_cursor;
            }

            if (select_unclicked && m_selected) {
                if (pickable_under_cursor) {
                    m_selection_controller.TryToConnectTo(pickable_under_cursor);
                } else {
                    m_selection_controller.AbortConnection();
                }
                m_selected = null;
            }
        } else if (m_selected) {
            float distance_in_plane = (m_pointer_ray.origin - m_selected.transform.position).magnitude;
            Vector3 mouse_pos = m_pointer_ray.origin + m_pointer_ray.direction * distance_in_plane;
            GameObject under_cursor = FindObjectUnderCursor(mouse_projection_distance);
            if (under_cursor) {
                // Snap to object
                mouse_pos = under_cursor.transform.position;
            }
            m_selection_controller.TryToDragConnectionTo(mouse_pos);
        }

        DebugMoveCursor();

    }

    void OnDrawGizmos () {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(m_start_position, m_end_position);

        Gizmos.color = Color.blue;
        Gizmos.DrawRay(m_pointer_ray);
    }

    public void SetCursorRay (Ray ray) {
        m_pointer_ray = ray;
    }

    public void SetCursorLine (Vector3 start_position, Vector3 end_position) {
        m_start_position = start_position;
        m_end_position = end_position;
    }

    // Return the game object the mouse is hovering over.
    // cast_distance: how far the object can be from the camera
    // debug: if true, displays the raycast briefly
    private GameObject FindObjectUnderCursor (float cast_distance, bool debug = false) {
        // TODO (Julian): Once we switch to the rift, we probably can't use the main camera.
        // We may even need to use a manual raycast that doesn't involve ScreenPointToRay
        RaycastHit hit;
        GameObject hit_object = null;
        if (debug) Debug.DrawRay(m_pointer_ray.origin, m_pointer_ray.direction * cast_distance, Color.yellow, 0.1f);
        if (Physics.Raycast(m_pointer_ray.origin, m_pointer_ray.direction, out hit, cast_distance)) {
            hit_object = hit.collider.gameObject;
        }
        return hit_object;
    }

    private void DebugMoveCursor () {
        m_debug_cursor_REMOVE_ME.transform.position = m_end_position;
    }

}
