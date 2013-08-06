using UnityEngine;
using System.Collections;

public class TwoDimGUI : MonoBehaviour {

    public Texture m_crosshair_texture;
    public ContinuousInputController m_continuous_input_controller;
    public Camera m_standard_camera;

    private Vector3 m_mouse_pos;

    // Use this for initialization
    void Start () {
        if (!m_crosshair_texture) {
            Debug.LogError("TwoDimGUI: Assign a Crosshair Texture in the inspector.");
            return;
        }

        // Make sure to hide mouse cursor
        if (Application.isEditor == false) {
            Screen.showCursor = false;
        }
    }

    // Update is called once per frame
    void Update () {
        m_mouse_pos = Input.mousePosition;
        //m_continuous_input_controller.SetCursorLine();
        Ray mouse_ray = m_standard_camera.ScreenPointToRay(Input.mousePosition);
        m_continuous_input_controller.SetCursorRay(ref mouse_ray);
    }

    void OnGUI() {
        GUI.DrawTexture(new Rect(m_mouse_pos.x - m_crosshair_texture.width/2.0f,
                                 Screen.height-m_mouse_pos.y - m_crosshair_texture.height/2.0f, m_crosshair_texture.width, m_crosshair_texture.height),
                        m_crosshair_texture);
    }
}
