using UnityEngine;
using System.Collections;

public class Mass : MonoBehaviour {
    public float m_mass;
    private float m_minimum_mass = 0.0f;

    public void InitializeMinimum (float mass) {
        m_minimum_mass = mass;
    }

    public void Initialize (float mass) {
        m_mass = mass;
    }

    public float Get () {
        return m_mass;
    }

    public bool TryToIncreaseBy (float mass) {
        m_mass += mass;
        return true;
    }

    public bool TryToDecreaseBy (float mass) {
        if (m_mass - mass >= m_minimum_mass) {
            m_mass -= mass;
            return true;
        }
        return false;
    }
}
