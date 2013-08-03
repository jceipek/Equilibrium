using UnityEngine;
using System.Collections;

public enum enDISPLAY_MODE {Rift, Standard};

public class RiftManager : MonoBehaviour {
    public enDISPLAY_MODE m_display_mode;

    public static RiftManager g;

    void Awake () {
        // Only allow one in a scene
        if (g != null) {
            DestroyImmediate(gameObject);
            return;
        }

        g = this;
    }
}
