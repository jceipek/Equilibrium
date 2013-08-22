using UnityEngine;
using System.Collections;

public class ControllerInputController : MonoBehaviour {

	public float m_RotationSpeed;
	public bool m_InvertY;

	// Update is called once per frame
	void Update ()
	{
		float inverter = -1.0f;
		if (m_InvertY)
		{
			inverter *= 1.0f;
		}

		float xInput = Input.GetAxis("Horizontal") * m_RotationSpeed;
		float yInput = Input.GetAxis("Vertical") * m_RotationSpeed * inverter;
		gameObject.transform.Rotate(xInput * Vector3.up * Time.deltaTime);
		gameObject.transform.Rotate(yInput * Vector3.right * Time.deltaTime);
	}
}
