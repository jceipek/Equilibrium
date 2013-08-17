using UnityEngine;
using System.Collections;

[RequireComponent (typeof (SelectionController))]
public class DiscreteInputController : MonoBehaviour {

    private SelectionController m_selection_controller;
    private Node m_selected = null;
    private Vector3 m_view_origin;
    private Vector3 m_view_forward;
    private Vector3 m_view_up;
    private Vector3 m_view_right;

    // Use this for initialization
    void Start () {
        if (!m_selected) {
            //GetClosestNodeInDirection ()
        }
        m_selection_controller = gameObject.GetComponent<SelectionController>();
    }

    // Update is called once per frame
    void Update () {

    }

    public void SetViewOrientation (Vector3 view_origin, Vector3 view_forward, Vector3 view_up, Vector3 view_right) {
        m_view_origin = view_origin;
        m_view_forward = view_forward;
        m_view_up = view_up;
        m_view_right = view_right;
    }

    private Node GetClosestNodeInDirection (Vector3 direction) {
        Debug.LogError("Method Not Implemented Yet"); // TODO (Julian, Alex): Implement this
        return null;
    }

}
