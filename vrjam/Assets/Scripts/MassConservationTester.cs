using UnityEngine;
using System.Collections;

public class MassConservationTester : MonoBehaviour {

    public bool m_update_snapshot_now;
    public float m_mass_snapshot;
    public float m_free_mass_snapshot;
    public float m_negative_mass;

    public float m_current_free_mass;
    public float m_current_mass;
    public bool m_update_now;
    public bool m_update_cycle;
    public float m_update_cycle_rate_seconds;

    void Update () {
        if (m_update_snapshot_now) {
            TakeSnapshot ();
            m_update_snapshot_now = false;
        }
        if (m_update_now) {
            UpdateCurrentValues ();
            m_update_now = false;
        }
        if (m_update_cycle) {
            StartCoroutine(WaitAndUpdate(m_update_cycle_rate_seconds));
        }
    }

    IEnumerator WaitAndUpdate (float seconds) {
        yield return new WaitForSeconds(seconds);
        UpdateCurrentValues();
    }

    public void TakeSnapshot () {
        UpdateValues(ref m_free_mass_snapshot, ref m_mass_snapshot);
    }

    public void UpdateCurrentValues () {
        UpdateValues(ref m_current_free_mass, ref m_current_mass);
        float mass_difference = m_current_mass - m_mass_snapshot;
        if (mass_difference >= 0.1f) {
            Debug.Log("C_ERROR: Mass was gained: " + mass_difference);
        } else if (mass_difference <= -0.1f) {
            Debug.Log("C_ERROR: Mass was lost: " + (-mass_difference));
        }
    }

    private void UpdateValues (ref float free_mass, ref float total_mass) {
        Mass[] mass_objects = FindObjectsOfType(typeof(Mass)) as Mass[];
        m_negative_mass = 0.0f;
        total_mass = 0.0f;
        free_mass = 0.0f;
        foreach (Mass mass_object in mass_objects) {
            total_mass += mass_object.Get();
            float new_free_mass = mass_object.GetAmountAvailable();
            if (new_free_mass < 0) {
                m_negative_mass += new_free_mass;
                total_mass -= new_free_mass;
            }
            free_mass += new_free_mass;
        }
    }
}