using UnityEngine;
using System;
using System.Collections;

public class Mass : MonoBehaviour {
    public float m_value;
    private float m_minimum_value = 0.0f;

    public class ViolatedConservation : Exception
    {
        public ViolatedConservation () {

        }

        public ViolatedConservation (string message) {

        }
    }

    public void InitializeMinimum (float mass) {
        m_minimum_value = mass;
    }

    // Unneeded?
    //public float GetMinimum () {
    //    return m_minimum_value;
    //}

    public void Initialize (float mass) {
        m_value = mass;
    }

    public float Get () {
        return m_value;
    }

    public bool TryToIncreaseBy (float mass) {
        m_value += mass;
        return true;
    }

    public bool TryToDecreaseBy (float mass) {
        if (m_value - mass >= m_minimum_value) {
            m_value -= mass;
            return true;
        }
        throw new ViolatedConservation(); // TODO: Move this out of here
        return false;
    }
}
