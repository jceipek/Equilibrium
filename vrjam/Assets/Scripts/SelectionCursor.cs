using UnityEngine;
using System.Collections;

public class SelectionCursor {

    public float m_speed;

    public Texture m_image_crosshair     = null;

    public OVRCameraController m_camera_controller = null;

    public float   m_fade_time           = 0.3f;
    public float   m_fade_scale          = 0.6f;
    public float   m_crosshair_distance  = 1.0f;

    private float  m_dead_zone_x         =  400.0f;
    private float  m_dead_zone_y         =   75.0f;

    private float  m_scale_speed_x        =   7.0f;
    private float  m_scale_speed_y        =   7.0f;

    private bool   m_collision_with_geometry;
    private float  m_fade_value;
    private Camera m_main_camera;

    private float  XL                 = 0.0f;
    private float  YL                 = 0.0f;

    private float  m_screen_width        = 1280.0f;
    private float  m_screen_height       =  800.0f;


    /*void Update () {
        float t = Time.deltaTime;
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");
        Vector3 position = transform.position;
        Vector3 delta_position = new Vector3(x * m_speed * t,
                                             y * m_speed * t,
                                             0.0f);
        transform.position = position + delta_position;
    }*/

    #region Public Functions

    // SetCrosshairTexture
    public void SetCrosshairTexture (ref Texture image) {
        m_image_crosshair = image;
    }

    // SetOVRCameraController
    public void SetOVRCameraController (ref OVRCameraController cameraController) {
        m_camera_controller = cameraController;
        m_camera_controller.GetCamera(ref m_main_camera);

        if(m_camera_controller.PortraitMode == true)
        {
            float tmp = m_dead_zone_x;
            m_dead_zone_x = m_dead_zone_y;
            m_dead_zone_y = tmp;
        }
    }

    //IsCrosshairVisible
    public bool IsCrosshairVisible() {
        return (m_fade_value > 0.0f);
    }

    // Init
    public void Init() {
        m_collision_with_geometry   = false;
        m_fade_value                 = 0.0f;

        m_screen_width  = Screen.width;
        m_screen_height = Screen.height;

        // Initialize screen location of cursor
        XL = m_screen_width * 0.5f;
        YL = m_screen_height * 0.5f;
    }

    // UpdateCrosshair
    public void UpdateCrosshair()
    {
        // Do not do these tests within OnGUI since they will be called twice
        CollisionWithGeometryCheck();
    }

    // OnGUICrosshair
    public void  OnGUICrosshair()
    {
        if (!m_collision_with_geometry)
            m_fade_value += Time.deltaTime / m_fade_time;
        else
            m_fade_value -= Time.deltaTime / m_fade_time;

        m_fade_value = Mathf.Clamp(m_fade_value, 0.0f, 1.0f);

        if ((m_image_crosshair != null) && (m_fade_value != 0.0f)) {
            GUI.color = new Color(1, 1, 1, m_fade_value * m_fade_scale);

            // Calculate X
            XL += Input.GetAxis("Mouse X") * m_scale_speed_x;
            if(XL < m_dead_zone_x) {
                XL = m_dead_zone_x - 0.001f;
            } else if (XL > (Screen.width - m_dead_zone_x)) {
                XL = m_screen_width - m_dead_zone_x + 0.001f;
            }

            // Calculate Y
            YL -= Input.GetAxis("Mouse Y") * m_scale_speed_y;
            if(YL < m_dead_zone_y) {
                //CursorOnScreen = false;
                if(YL < 0.0f) YL = 0.0f;
            }
            else if (YL > m_screen_height - m_dead_zone_y) {
                //CursorOnScreen = false;
                if(YL > m_screen_height) YL = m_screen_height;
            }

            // Finally draw cursor
            bool allow_mouse_rotation = true;

            if(allow_mouse_rotation == true) {
                // Left
                GUI.DrawTexture(new Rect(   XL - (m_image_crosshair.width * 0.5f),
                                            YL - (m_image_crosshair.height * 0.5f),
                                            m_image_crosshair.width,
                                            m_image_crosshair.height),
                                            m_image_crosshair);
            }

            GUI.color = Color.white;
        }
    }
    #endregion

    #region Private Functions

    // m_collision_with_geometry
    bool CollisionWithGeometryCheck ()
    {
        m_collision_with_geometry = false;

        Vector3 start_position = m_main_camera.transform.position;
        Vector3 direction = Vector3.forward;
        direction = m_main_camera.transform.rotation * direction;
        direction *= m_crosshair_distance;
        Vector3 end_position = start_position + direction;

        RaycastHit hit;
        if (Physics.Linecast(start_position, end_position, out hit))
        {
            if (!hit.collider.isTrigger)
            {
                m_collision_with_geometry = true;
            }
        }

        return m_collision_with_geometry;
    }
    #endregion
}