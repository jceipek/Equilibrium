using UnityEngine;
using System.Collections;

public class TrailController : MonoBehaviour {

	public Vector3 m_velocity;
	public float m_life;

	// Update is called once per frame
	void Update () {
		transform.position += m_velocity * Time.deltaTime;
		m_life -= Time.deltaTime;
		if (m_life <= 0.0f) {
			Destroy(gameObject);
		}
	}
}
