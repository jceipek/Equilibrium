using UnityEngine;
using System.Collections;

public class ConnectToAvatar : MonoBehaviour {

	private CameraSwitcher m_CameraSwitcher;
	// Use this for initialization
	void OnEnable () {
		Debug.Log("WHYYYYY");
		GameObject player = GameObject.FindWithTag("Player");
		m_CameraSwitcher = player.GetComponent<CameraSwitcher>();
		m_CameraSwitcher.ConnectRiftCamera(gameObject);
	}
}
