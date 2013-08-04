using UnityEngine;
using System.Collections;

public class MassVisualizationController : MonoBehaviour {

    public Mass m_mass_component;

    void Update () {
        // Potential Optimization is to compute this only when mass changes
        gameObject.transform.localScale = Vector3.one * m_mass_component.m_mass * RulesManager.g.m_MASS_TO_SIZE_RATIO;
    }
}