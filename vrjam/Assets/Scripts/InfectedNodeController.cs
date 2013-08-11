using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InfectedNodeController : MonoBehaviour {

    public GameObject m_visuals; // NEED TO REMOVE FROM HERE!
    private Dictionary<Node, float> m_infection_percentages;
    private Node m_node;

    void OnEnable () {
        m_infection_percentages = new Dictionary<Node, float>();
        m_node = gameObject.GetComponent<Node>();
    }

    // Update is called once per frame
    void Update () {
        // Can Refactor for more efficiency (connections instead of nodes)
        foreach (Node node in m_node.GetConnectedNodes()) {
            if (!m_infection_percentages.ContainsKey(node)) {
                m_infection_percentages.Add(node, 0.0f);
            } else {
                if (m_infection_percentages[node] < 1.0f) {
                    m_infection_percentages[node] += RulesManager.g.m_INFECTION_SPEED *
                                                           Time.deltaTime;
                } else {
                    Infect(node);
                }
            }
        }
        TEMP_UpdateVisuals();
    }

    public float GetInfectionPercentForNode (Node node) {
        if (m_infection_percentages != null) {
            if (!m_infection_percentages.ContainsKey(node)) {
                return 0.0f;
            }
            return m_infection_percentages[node];
        }
        return 0.0f;
    }

    public void FullyInfect (Node node) {
        if (m_infection_percentages != null) {
            m_infection_percentages[node] = 1.0f;
        }
    }

    private void TEMP_UpdateVisuals () {
        /*foreach (Node node in m_node.GetConnectedNodes()) {
            Connection connection = m_node.ConnectionSharedWith(node);
            Color.Lerp(Color.red, Color.black, m_infection_percentages[node]);
        }*/
        m_visuals.renderer.material.color = Color.black;
    }

    private void Infect (Node node) {
        InfectedNodeController other_infection = node.GetComponent<InfectedNodeController>();
        other_infection.enabled = true;
        other_infection.FullyInfect(m_node);
    }
}
