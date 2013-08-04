using UnityEngine;
using System.Collections;

public class Mass : MonoBehaviour {
    public float m_mass;

    public void Initialize (float mass) {
        m_mass = mass;
    }

    public float Get () {
        return m_mass;
    }

    public void IncreaseBy (float mass) {
        m_mass += mass;
    }

    public void DecreaseBy (float mass) {
        m_mass -= mass;
    }
}
