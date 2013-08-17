using UnityEngine;
using System.Collections;

public class TrailController : MonoBehaviour {

	private Vector3 m_velocity;
	private float m_life;
	private Color m_color;

	// Update is called once per frame
	void Update () {
		transform.position += m_velocity * Time.deltaTime;
		m_life -= Time.deltaTime;
		if (m_life <= 0.0f) {
			Destroy(gameObject);
		}
	}

	public void InitializeWith(Color color, Vector3 velocity, float life) {
		m_velocity = velocity;
		m_life = life;
		m_color = color;
		renderer.material.SetColor ("_TintColor", m_color);
		//renderer.material.color = m_color;
	}
}
