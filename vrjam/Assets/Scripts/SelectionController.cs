using UnityEngine;
using System.Collections;

public class SelectionController : MonoBehaviour {

    public Camera m_main_camera; // TODO (Julian): Generalize this to the non-rift camera
    public Pickable m_selected_object = null;
    public Camera m_standard_camera;

    private float rift_dead_zone_x =  400.0f;
    private float rift_dead_zone_y = 75.0f;

    void Start () {

    }

    void Update () {
        bool select_clicked = Input.GetButtonDown("Select");
        bool select_unclicked = Input.GetButtonUp("Select");
        if (select_clicked || select_unclicked) {
            Pickable pickable_under_mouse = null;
            GameObject under_mouse = FindObjectUnderMouse(cast_distance: 100, debug: true);
            if (under_mouse) pickable_under_mouse = under_mouse.GetComponent<Pickable>();

            if (select_clicked && pickable_under_mouse) {
                m_selected_object = pickable_under_mouse;
            }

            if (select_unclicked && m_selected_object) {
                if (pickable_under_mouse) {
                    // TODO (Julian): Connect m_selected_object and pickable_under_mouse properly
                    // XXX (Julian): This strong coupling is bad. Not sure how to handle this.
                    m_selected_object.ConnectTo(pickable_under_mouse);
                    m_selected_object = null;
                } else {
                    // TODO (Julian): Actually release connection properly
                    m_selected_object = null;
                }
            }
        }
    }

    void OnDrawGizmos () {
        if (IsAnObjectSelected()) {
            Ray ray = m_standard_camera.ScreenPointToRay(Input.mousePosition);
            Gizmos.DrawLine(m_selected_object.transform.position, ray.origin + ray.direction * 100);
        }
    }

    // Is an object already selected?
    public bool IsAnObjectSelected () {
        return (m_selected_object != null);
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