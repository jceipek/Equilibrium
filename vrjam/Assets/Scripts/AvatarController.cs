using UnityEngine;
using System.Collections;

public class AvatarController : MonoBehaviour {

	public GameObject m_rift_camera;
	public GameObject m_standard_camera;

	void Start() {
		if (RiftManager.g.m_display_mode == enDISPLAY_MODE.Rift) {
			SwitchToRiftCam();
		} else {
			SwitchToStandardCam();
		}
	}

	private void SwitchToStandardCam () {
        m_rift_camera.SetActive(false);
        m_standard_camera.SetActive(true);
    }

    private void SwitchToRiftCam () {
        m_rift_camera.SetActive(true);
        m_standard_camera.SetActive(false);
    }
}
