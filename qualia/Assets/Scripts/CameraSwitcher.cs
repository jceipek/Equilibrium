using UnityEngine;
using System.Collections;

public enum CAMERA_MODE {Rift, Standard};

public class CameraSwitcher : MonoBehaviour {

    public CAMERA_MODE m_CameraMode;
    private GameObject m_RiftCamera;
    public GameObject m_StandardCamera;
    public AudioListener m_StandardListener;
    private AudioListener m_RiftListener;

    private Vector3 m_Offset;
    private bool m_OffsetSet = false;

    public void ConnectRiftCamera (GameObject riftCamera) {
        Debug.Log("Connecting RIFT Camera");
        m_RiftCamera = riftCamera;
        Debug.Log(m_RiftCamera);
        m_RiftListener = m_RiftCamera.GetComponentInChildren<AudioListener>();
        Debug.Log(m_RiftListener);
        m_RiftCamera.transform.parent = gameObject.transform;
        if (!m_OffsetSet)
        {
            m_Offset = m_RiftCamera.transform.position - gameObject.transform.position;
            m_OffsetSet = true;
        }
        else
        {
            m_RiftCamera.transform.position = gameObject.transform.position + m_Offset;
        }

        switch (m_CameraMode)
        {
            case CAMERA_MODE.Rift:
                SwitchToRiftCamera();
                break;
            case CAMERA_MODE.Standard:
                SwitchToStandardCamera();
                break;
            default:
                break;
        }
    }

    public void DisconnectRiftCamera () {
        Destroy(m_RiftCamera);
        //m_RiftCamera.transform.parent = null;
        m_RiftCamera = null;
    }

    public void SwitchToRiftCamera () {
        m_StandardListener.enabled = false;
        m_RiftListener.enabled = true;
        m_StandardCamera.SetActive(false);
        m_RiftCamera.SetActive(true);
    }

    public void SwitchToStandardCamera () {
        m_StandardListener.enabled = true;
        m_RiftListener.enabled = false;
        m_RiftCamera.SetActive(false);
        m_StandardCamera.SetActive(true);
    }
}