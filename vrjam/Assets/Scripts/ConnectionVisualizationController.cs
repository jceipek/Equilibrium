using UnityEngine;
using System.Collections;

[RequireComponent (typeof (LineRenderer))]
public class ConnectionVisualizationController : MonoBehaviour {

    public Connection m_connection_component;

    void Update () {
        // Potential Optimization: Do this only when the Connection updates
        Node start = m_connection_component.GetStartNode();
        Node end = m_connection_component.GetEndNode();
        Vector3 end_point = m_connection_component.GetEndPoint();
        if (start != null) {
            LineRenderer renderer = gameObject.GetComponent<LineRenderer>();
            renderer.SetPosition(0, start.gameObject.transform.position);
            if (end != null) {
                renderer.SetPosition(1, end.gameObject.transform.position);
            } else {
                renderer.SetPosition(1, end_point);
            }
        }
    }
}