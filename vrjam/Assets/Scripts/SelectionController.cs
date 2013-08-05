using UnityEngine;
using System.Collections;

public class SelectionController : MonoBehaviour {

    public Camera m_main_camera; // TODO (Julian): Generalize these (Rift doesn't even work)
    public Camera m_standard_camera;

    public Connection m_connection = null;

    //private float rift_dead_zone_x =  400.0f;
    //private float rift_dead_zone_y = 75.0f;

    void Start () {

    }

    void Update () {
        float mouse_projection_distance = 100.0f;

        bool select_clicked = Input.GetButtonDown("Select");
        bool select_unclicked = Input.GetButtonUp("Select");
        if (select_clicked || select_unclicked) {
            Node pickable_under_mouse = null;
            GameObject under_mouse = FindObjectUnderMouse(cast_distance: mouse_projection_distance, debug: true);
            if (under_mouse) pickable_under_mouse = under_mouse.GetComponent<Node>();

            if (select_clicked && pickable_under_mouse) {
                SelectNode(pickable_under_mouse);
            }

            if (select_unclicked && m_connection) {
                if (pickable_under_mouse) {
                    TryToConnectTo(pickable_under_mouse);
                } else {
                    AbortConnection();
                }
            }
        } else if (m_connection) {
            float distance_in_plane = (m_standard_camera.transform.position - m_connection.GetStartNode().transform.position).magnitude;
            Ray ray = m_standard_camera.ScreenPointToRay(Input.mousePosition);
            Vector3 mouse_pos = ray.origin + ray.direction * distance_in_plane;
            GameObject under_mouse = FindObjectUnderMouse(mouse_projection_distance);
            if (under_mouse) {
                // Snap to object
                mouse_pos = under_mouse.transform.position;
            }
            TryToDragConnectionTo(mouse_pos);
        }
    }

    public bool IsANodeSelected () {
        return (m_connection != null);
    }

    public void SelectNode (Node node) {
        GameObject connection = (GameObject)Instantiate(Resources.Load("Connection"));
        Connection connection_component = connection.GetComponent<Connection>();
        connection_component.InitializeWithStartNode(node);
        m_connection = connection_component;
    }

    public bool TryToConnectTo (Node node) {
        bool success = m_connection.TryToFinishConnectionWithEndNode(node);
        if (!success) m_connection.DestroyConnection(); // TODO (Julian): Maybe move this into TryToFinishConnectionWithEndNode?
        m_connection = null;
        return success;
    }

    public bool TryToDragConnectionTo (Vector3 position) {
        return m_connection.TryToReachPoint(position);
    }

    public void AbortConnection () {
        m_connection.DestroyConnection();
        m_connection = null;
    }

    // Return the game object the mouse is hovering over.
    // cast_distance: how far the object can be from the camera
    // debug: if true, displays the raycast briefly
    private GameObject FindObjectUnderMouse (float cast_distance, bool debug = false) {
        // TODO (Julian): Once we switch to the rift, we probably can't use the main camera.
        // We may even need to use a manual raycast that doesn't involve ScreenPointToRay
        Ray ray = m_standard_camera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        GameObject hit_object = null;
        if (debug) Debug.DrawRay(ray.origin, ray.direction * cast_distance, Color.yellow, 0.1f);
        if (Physics.Raycast(ray.origin, ray.direction, out hit, cast_distance)) {
            hit_object = hit.collider.gameObject;
        }
        return hit_object;
    }

    // Uses the Rift, needs to be merged with other method at some point soon?
    /*private GameObject NewFindObjectUnderMouse (float cast_distance) {
        float x = Input.mousePosition.x  ScreenWidth - DeadZoneX + 0.001f;
        float y = Input.mousePosition.y;
        Screen.width;
        Screen.height;

        Vector3 start_position = m_main_camera.transform.position;
        Vector3 dir = Vector3.forward;
        dir = m_main_camera.transform.rotation * dir;
        dir *= cast_distance;
        Vector3 end_position = start_position + dir;

        RaycastHit hit;
        GameObject hit_object = null;
        if (Physics.Linecast(start_position, end_position, out hit))
        {
            hit_object = hit.collider.gameObject;
        }

        return hit_object;
    }*/
}
