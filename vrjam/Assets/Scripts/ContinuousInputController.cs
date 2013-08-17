using UnityEngine;
using System.Collections;

[RequireComponent (typeof (SelectionController))]
public class ContinuousInputController : MonoBehaviour {

    private SelectionController m_selection_controller;

    private Vector3 m_start_position;
    private Vector3 m_end_position;

    private Ray m_pointer_ray;

    private Node m_selected = null;
    private Node m_target = null;
    private static LayerMask m_SELECTABLE_LAYER;

    public GameObject m_debug_cursor_REMOVE_ME;

    void OnEnable () {
        m_SELECTABLE_LAYER = (1 << LayerMask.NameToLayer("Selectable"));
    }

    void Start () {
        m_selection_controller = gameObject.GetComponent<SelectionController>();
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
        if (Physics.Raycast(m_pointer_ray.origin, m_pointer_ray.direction, out hit, cast_distance, m_SELECTABLE_LAYER)) {
            hit_object = hit.collider.gameObject;
        }
        return hit_object;
    }

    private void DebugMoveCursor () {
        m_debug_cursor_REMOVE_ME.transform.position = m_end_position;
    }

}
