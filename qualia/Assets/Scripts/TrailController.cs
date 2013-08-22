using UnityEngine;
using System.Collections;

public class TrailController : MonoBehaviour {

	private Vector3 m_Velocity;
	private float m_Life;
	private Color m_Color;

	// Update is called once per frame
	void Update () {
		transform.position += m_Velocity * Time.deltaTime;
		m_Life -= Time.deltaTime;
		if (m_Life <= 0.0f) {
			Destroy(gameObject);
		}
	}

	public void InitializeWith(Color color, Vector3 velocity, float life) {
		m_Velocity = velocity;
		m_Life = life;
		m_Color = color;
		renderer.material.SetColor ("_TintColor", m_Color);
		//renderer.material.color = m_Color;
	}
}