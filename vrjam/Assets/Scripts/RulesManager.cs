using UnityEngine;
using System.Collections;

public class RulesManager : MonoBehaviour {
    public static RulesManager g;

    public float m_MASS_TO_SIZE_RATIO = 0.54f;
    public float m_TRANSFER_SPEED = 2.0f;

    void Awake () {
        // Only allow one in a scene
        if (g != null) {
            DestroyImmediate(gameObject);
            return;
        }

        g = this;
    }
}