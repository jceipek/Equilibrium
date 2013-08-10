using UnityEngine;
using System.Collections;

public class RulesManager : MonoBehaviour {
    public static RulesManager g;

    public float m_INFECTION_SPEED = 0.1f;
    public float m_MASS_TO_SIZE_RATIO = 0.54f; // May replace with proper volume calculation
    public float m_MASS_TO_LENGTH_RATIO = 0.54f;
    public float m_MASS_TO_LIGHT_RATIO = 0.01f;
    public float m_DISTANCE_TO_SPEED_RATIO = 2.0f;
    public float m_TRANSFER_SPEED = 2.0f;
    public float m_MINIMUM_NODE_MASS = 2.0f;

    void Awake () {
        // Only allow one in a scene
        if (g != null) {
            DestroyImmediate(gameObject);
            return;
        }

        g = this;
    }
}