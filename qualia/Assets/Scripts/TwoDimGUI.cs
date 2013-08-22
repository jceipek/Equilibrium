using UnityEngine;
using System.Collections;

public class TwoDimGUI : MonoBehaviour
{
    public GameObject m_Target;
    public Texture m_CrosshairTexture;
    private Camera m_Camera;

    private Vector3 m_mouse_pos;

    void OnEnable ()
    {
        m_Camera = GetComponentInChildren<Camera>();
        if (!m_CrosshairTexture)
        {
            Debug.LogError("TwoDimGUI: Assign a Crosshair Texture in the inspector.");
            return;
        }
        // Make sure to hide mouse cursor
        if (Application.isEditor == false)
        {
            Screen.showCursor = false;
        }
    }

    void Update ()
    {
        Vector3 startPosition = m_Camera.transform.position;
        Vector3 direction = Vector3.forward;
        direction = m_Camera.transform.rotation * direction;
        Vector3 endPosition = startPosition + direction;

        m_Target.transform.position = endPosition;
        m_Target.transform.localRotation = m_Camera.transform.rotation;
    }

    void OnGUI() {
        GUI.DrawTexture(new Rect(Screen.width/2.0f - m_CrosshairTexture.width/2.0f,
                                 Screen.height/2.0f - m_CrosshairTexture.height/2.0f, m_CrosshairTexture.width, m_CrosshairTexture.height),
                        m_CrosshairTexture);
    }
}
