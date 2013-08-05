using UnityEngine;
using System.Collections;

[RequireComponent (typeof (SelectionController))]
public class ContinuousInputController : MonoBehaviour {

    private Vector3 m_start_position;
    private Vector3 m_end_position;

    // Use this for initialization
    void Start () {
    
    }
    
    // Update is called once per frame
    void Update () {
    
    }

    void OnDrawGizmos () {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(m_start_position, m_end_position);
    }

    public void SetCursorLine (Vector3 start_position, Vector3 end_position) {
        m_start_position = start_position;
        m_end_position = end_position;
    }

}
