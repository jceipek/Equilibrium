using UnityEngine;
using System.Collections;

public class TwoDimGUI : MonoBehaviour {

    public Texture m_crosshair_texture;
    public ContinuousInputController m_continuous_input_controller;
    public DiscreteInputController m_discrete_input_controller;

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
        m_continuous_input_controller.SetCursorRay(mouse_ray);

        Vector3 camera_origin = m_standard_camera.transform.position;
        Vector3 camera_up = m_standard_camera.transform.up;
        Vector3 camera_right = m_standard_camera.transform.right;
        Vector3 camera_forward = m_standard_camera.transform.forward;
        m_discrete_input_controller.SetViewOrientation(view_origin: camera_origin,
                                                       view_forward: camera_forward,
                                                       view_up: camera_up,
                                                       view_right: camera_right);
    }

    void OnGUI() {
        GUI.DrawTexture(new Rect(m_mouse_pos.x - m_crosshair_texture.width/2.0f,
                                 Screen.height-m_mouse_pos.y - m_crosshair_texture.height/2.0f, m_crosshair_texture.width, m_crosshair_texture.height),
                        m_crosshair_texture);
    }
}
