using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(Node))]
public class Infected : MonoBehaviour
{
    private Node m_Node;
    private Dictionary<Node, float> m_ConnectedNodeToInfectionPercentage;

    void OnEnable ()
    {
        m_Node = GetComponent<Node>();
        m_ConnectedNodeToInfectionPercentage = new Dictionary<Node, float>();
    }

    void Update ()
    {
        foreach (Node node in m_Node.GetConnectedNodes()) {
            if (!m_ConnectedNodeToInfectionPercentage.ContainsKey(node))
            {
                m_ConnectedNodeToInfectionPercentage[node] = 0.0f;
            }
            if (m_ConnectedNodeToInfectionPercentage[node] >= 1.0f) {
                Infect(node);
            }
            else
            {
                float distance = (m_Node.transform.position - node.transform.position).magnitude;
                m_ConnectedNodeToInfectionPercentage[node] += 1.0f/distance * Constants.g.INFECTION_SPREAD_SPEED * Time.deltaTime;
                m_ConnectedNodeToInfectionPercentage[node] = Mathf.Min(1.0f, m_ConnectedNodeToInfectionPercentage[node]);
                Connection connection = m_Node.GetConnectionWith(node);
                ConnectionVisualizer connectionVisuals = connection.GetComponentInChildren<ConnectionVisualizer>();
                // TODO (JULIAN): Consider uncommenting this
                //connectionVisuals.UpdateInfection();
            }
        }
    }

    public float GetInfectionPercentFor(Node node)
    {
        if (m_ConnectedNodeToInfectionPercentage.ContainsKey(node))
        {
            return m_ConnectedNodeToInfectionPercentage[node];
        }
        return 0.0f;
    }

    private void Infect (Node node)
    {
        Infected infected = node.GetComponent<Infected>();
        infected.enabled = true;
        NodeVisualizer nodeVisuals = node.GetComponentInChildren<NodeVisualizer>();
        // TODO (JULIAN): Consider uncommenting this
        //nodeVisuals.UpdateInfection();
    }
}
