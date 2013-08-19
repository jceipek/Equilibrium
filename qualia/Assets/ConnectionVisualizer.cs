using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(LineRenderer))]
public class ConnectionVisualizer : MonoBehaviour
{
    private LineRenderer m_lineRenderer;
    private Node m_Start;
    private Node m_End;

    private Infected m_StartInfection;
    private Infected m_EndInfection;

    void OnEnable ()
    {
        m_lineRenderer = GetComponent<LineRenderer>();
    }

    public void InitializeWithNodes (Node start, Node end)
    {
        m_Start = start;
        m_End = end;

        m_StartInfection = m_Start.GetComponentInChildren<Infected>();
        m_EndInfection = m_End.GetComponentInChildren<Infected>();

        m_lineRenderer.SetPosition(0, m_Start.transform.position);
        m_lineRenderer.SetPosition(1, m_End.transform.position);
    }

    public void UpdateInfection ()
    {
        Color startColor = Constants.g.CONNECTION_NORMAL_COLOR;
        Color endColor = Constants.g.CONNECTION_NORMAL_COLOR;

        float startSpread = 0.0f;
        if (m_StartInfection.enabled)
        {
            startSpread = m_StartInfection.GetInfectionPercentFor(m_End);

            startColor = Color.Lerp(Constants.g.CONNECTION_NORMAL_COLOR, Constants.g.CONNECTION_INFECTED_COLOR, startSpread/0.5f);
            endColor = Color.Lerp(Constants.g.CONNECTION_NORMAL_COLOR, Constants.g.CONNECTION_INFECTED_COLOR, startSpread/0.5f - 1.0f);
        }

        float endSpread = 0.0f;
        if (m_EndInfection.enabled)
        {
            endSpread = m_EndInfection.GetInfectionPercentFor(m_Start);

            startColor = Color.Lerp(Constants.g.CONNECTION_NORMAL_COLOR, Constants.g.CONNECTION_INFECTED_COLOR, endSpread/0.5f - 1.0f);
            endColor = Color.Lerp(Constants.g.CONNECTION_NORMAL_COLOR, Constants.g.CONNECTION_INFECTED_COLOR, endSpread/0.5f);
        }

        if (m_StartInfection.enabled && m_EndInfection.enabled)
        {
            startColor = Color.Lerp(Constants.g.CONNECTION_NORMAL_COLOR, Constants.g.CONNECTION_INFECTED_COLOR, startSpread/0.5f + endSpread/0.5f - 1.0f);
            endColor = Color.Lerp(Constants.g.CONNECTION_NORMAL_COLOR, Constants.g.CONNECTION_INFECTED_COLOR, endSpread/0.5f + startSpread/0.5f - 1.0f);
        }

        m_lineRenderer.SetColors(startColor, endColor);
    }
}