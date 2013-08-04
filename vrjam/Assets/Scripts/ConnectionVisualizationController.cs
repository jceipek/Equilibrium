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
            Mass start_mass = start.GetComponent<Mass>();
            float start_width = start_mass.Get() * RulesManager.g.m_MASS_TO_SIZE_RATIO / 2.0f;
            if (end != null) {
                Mass end_mass = end.GetComponent<Mass>();
                float end_width = start_mass.Get() * RulesManager.g.m_MASS_TO_SIZE_RATIO / 2.0f;
                renderer.SetPosition(1, end.gameObject.transform.position);
                renderer.SetWidth(start_width, end_width);
            } else {
                renderer.SetPosition(1, end_point);
                renderer.SetWidth(start_width, start_width);
            }
        }
    }
}