using UnityEngine;
using System.Collections;

public class AvatarController : MonoBehaviour {

	public GameObject riftCam;
	public GameObject stdCam;

	void Start() {
		if (RiftManager.g.m_display_mode == enDISPLAY_MODE.Rift) {
			SwitchToRiftCam();
		} else {
			SwitchToStandardCam();
		}
	}

	private void SwitchToStandardCam () {
        riftCam.SetActive(false);
        stdCam.SetActive(true);
    }

    private void SwitchToRiftCam () {
        riftCam.SetActive(true);
        stdCam.SetActive(false);
    }
}
