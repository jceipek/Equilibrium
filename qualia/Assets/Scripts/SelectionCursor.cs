using UnityEngine;
using System.Collections;

public class SelectionCursor
{
    public Texture m_ImageCrosshair     = null;

    public OVRCameraController m_CameraController = null;

    public float   m_FadeTime           = 0.3f;
    public float   m_FadeScale          = 0.6f;
    public float   m_CrosshairDistance  = 1.0f;

    private float  m_DeadZoneX         =  400.0f;
    private float  m_DeadZoneY         =   75.0f;

    private bool   m_CollisionWithGeometry;
    private float  m_FadeValue;
    private Camera m_MainCamera;
    private GameObject m_Target;

    private float  m_XL                 = 0.0f;
    private float  m_YL                 = 0.0f;

    private float  m_ScreenWidth        = 1280.0f;
    private float  m_ScreenHeight       =  800.0f;

    #region Public Functions

    // SetTarget
    public void SetTarget (ref GameObject target)
    {
        m_Target = target;
    }

    // SetCrosshairTexture
    public void SetCrosshairTexture (ref Texture image)
    {
        m_ImageCrosshair = image;
    }

    // SetOVRCameraController
    public void SetOVRCameraController (ref OVRCameraController camera_controller)
    {
        m_CameraController = camera_controller;
        m_CameraController.GetCamera(ref m_MainCamera);

        if(m_CameraController.PortraitMode == true)
        {
            float tmp = m_DeadZoneX;
            m_DeadZoneX = m_DeadZoneY;
            m_DeadZoneY = tmp;
        }
    }

    /*public void SetContinuousInputController (ref ContinuousInputController continuous_input_controller) {
        m_continuous_input_controller = continuous_input_controller;
    }*/

    //IsCrosshairVisible
    public bool IsCrosshairVisible()
    {
        return (m_FadeValue > 0.0f);
    }

    // Init
    public void Init()
    {
        m_CollisionWithGeometry   = false;
        m_FadeValue                 = 0.0f;

        m_ScreenWidth  = Screen.width;
        m_ScreenHeight = Screen.height;

        // Initialize screen location of cursor
        m_XL = m_ScreenWidth * 0.5f;
        m_YL = m_ScreenHeight * 0.5f;
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
        if (!m_CollisionWithGeometry)
            m_FadeValue += Time.deltaTime / m_FadeTime;
        else
            m_FadeValue -= Time.deltaTime / m_FadeTime;

        m_FadeValue = Mathf.Clamp(m_FadeValue, 0.0f, 1.0f);

        if ((m_ImageCrosshair != null) && (m_FadeValue != 0.0f))
        {
            GUI.color = new Color(1, 1, 1, m_FadeValue * m_FadeScale);

            // Calculate X
            if(m_XL < m_DeadZoneX)
            {
                m_XL = m_DeadZoneX - 0.001f;
            }
            else if (m_XL > (Screen.width - m_DeadZoneX))
            {
                m_XL = m_ScreenWidth - m_DeadZoneX + 0.001f;
            }

            // Calculate Y
            if(m_YL < m_DeadZoneY)
            {
                //CursorOnScreen = false;
                if(m_YL < 0.0f) m_YL = 0.0f;
            }
            else if (m_YL > m_ScreenHeight - m_DeadZoneY)
            {
                //CursorOnScreen = false;
                if(m_YL > m_ScreenHeight) m_YL = m_ScreenHeight;
            }

            // Finally draw cursor
            bool allow_mouse_rotation = true;

            if(allow_mouse_rotation == true)
            {
                // Left
                GUI.DrawTexture(new Rect(   m_XL - (m_ImageCrosshair.width * 0.5f),
                                            m_YL - (m_ImageCrosshair.height * 0.5f),
                                            m_ImageCrosshair.width,
                                            m_ImageCrosshair.height),
                                            m_ImageCrosshair);
            }

            GUI.color = Color.white;
        }
    }
    #endregion

    #region Private Functions

    // m_CollisionWithGeometry
    bool CollisionWithGeometryCheck ()
    {
        m_CollisionWithGeometry = false;

        Vector3 startPosition = m_MainCamera.transform.position;
        Vector3 direction = Vector3.forward;
        direction = m_MainCamera.transform.rotation * direction;
        direction *= m_CrosshairDistance;
        Vector3 endPosition = startPosition + direction;

        if (m_Target)
        {
            m_Target.transform.position = endPosition;
            m_Target.transform.localRotation = m_MainCamera.transform.rotation;
        }

        RaycastHit hit;
        if (Physics.Linecast(startPosition, endPosition, out hit))
        {
            if (!hit.collider.isTrigger)
            {
                m_CollisionWithGeometry = true;
            }
        }

        return m_CollisionWithGeometry;
    }
    #endregion
}