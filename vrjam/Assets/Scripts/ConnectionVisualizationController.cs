using UnityEngine;
using System.Collections;

[RequireComponent (typeof (LineRenderer))]
public class ConnectionVisualizationController : MonoBehaviour {

    public Connection m_connection_component;
    public Color m_normal_color;
    public Color m_infected_color;

    void Update () {
        // Potential Optimization: Do this only when the Connection updates
        Node start = m_connection_component.GetStartNode();
        Node end = m_connection_component.GetEndNode();

        float start_infection_percent = 0.0f;
        float end_infection_percent = 0.0f;
        if (start) {
            InfectedNodeController start_infection = start.GetComponent<InfectedNodeController>();
            if (end) {
                start_infection_percent = start_infection.GetInfectionPercentForNode(end);
            }
        }
        if (end) {
            InfectedNodeController end_infection = end.GetComponent<InfectedNodeController>();
            if (start) {
                end_infection_percent = end_infection.GetInfectionPercentForNode(start);
            }
        }

        Vector3 end_point = m_connection_component.GetEndPoint();
        if (start != null) {
            LineRenderer renderer = gameObject.GetComponent<LineRenderer>();
            renderer.SetPosition(0, start.gameObject.transform.position);
            //float start_width = 1.0f;
            if (end != null) {
                renderer.SetPosition(1, end.gameObject.transform.position);
                //renderer.SetWidth(start_width, end_width);

                Color start_color = Color.Lerp(m_normal_color, m_infected_color, start_infection_percent);
                Color end_color = Color.Lerp(m_normal_color, m_infected_color, end_infection_percent);
                renderer.SetColors(start_color, end_color);
            } else {
                renderer.SetPosition(1, end_point);
                //renderer.SetWidth(start_width, start_width);
            }
        }
    }
}