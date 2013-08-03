using UnityEngine;
using System.Collections;

public class SelectionController : MonoBehaviour {

    public Pickable m_selected_object = null;

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
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
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
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        GameObject hit_object = null;
        if (debug) Debug.DrawRay(ray.origin, ray.direction * cast_distance, Color.yellow, 0.1f);
        if (Physics.Raycast(ray.origin, ray.direction, out hit, cast_distance)) {
            hit_object = hit.collider.gameObject;
        }
        return hit_object;
    }
}
