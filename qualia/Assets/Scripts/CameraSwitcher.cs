using UnityEngine;
using System.Collections;

public enum CAMERA_MODE {Rift, Standard};

public class CameraSwitcher : MonoBehaviour {

    public CAMERA_MODE m_CameraMode;
    public GameObject m_RiftCamera;
    public GameObject m_StandardCamera;

    void OnEnable ()
    {
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

    public void SwitchToRiftCamera () {
        m_StandardCamera.SetActive(false);
        m_RiftCamera.SetActive(true);
    }

    public void SwitchToStandardCamera () {
        m_RiftCamera.SetActive(false);
        m_StandardCamera.SetActive(true);
    }
}