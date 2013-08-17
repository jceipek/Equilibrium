using UnityEngine;
using System.Collections;

public class MassVisualizationController : MonoBehaviour {

    public Light m_light;

    //void Update () {
        // Potential Optimization is to compute this only when mass changes
        //gameObject.transform.localScale = Vector3.one * m_mass.Get() * RulesManager.g.m_MASS_TO_SIZE_RATIO;
        //m_light.intensity = m_mass.GetAmountAvailable() * RulesManager.g.m_MASS_TO_LIGHT_RATIO;
    //}
}