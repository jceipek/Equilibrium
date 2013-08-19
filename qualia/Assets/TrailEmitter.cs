using UnityEngine;
using System.Collections;

public class TrailEmitter : MonoBehaviour {

    public float m_Velocity;
    public float m_RefreshFrequency;
    public Color m_NormalColor;
    public Color m_InfectedColor;

	private Node m_Node;
	private Infected m_Infection;

	// Use this for initialization
    void Start () {
        StartCoroutine(EmissionTimer());
    }

    public void InitializeWithNode (Node node)
    {
        m_Node = node;
        m_Infection = node.GetComponentInChildren<Infected>();
    }

	IEnumerator EmissionTimer () {
        yield return new WaitForSeconds(m_RefreshFrequency);
        Emit();
        StartCoroutine(EmissionTimer());
    }

	private void Emit () {
        foreach (Node targetNode in m_Node.GetConnectedNodes()) {
            Vector3 emissionLocation = gameObject.transform.position + Random.insideUnitSphere * 0.5f;
            Vector3 positionDifference = (targetNode.transform.position - m_Node.transform.position);
            float distance = positionDifference.magnitude;
            GameObject trail = Instantiate(Resources.Load("Trail"), emissionLocation, gameObject.transform.rotation) as GameObject;
            TrailController trailController = trail.GetComponent<TrailController>();
            float life;
            if (m_Velocity <= 0.0f) {
                continue;
            } else {
                life = distance/m_Velocity;
            }
            Vector3 velocity = positionDifference.normalized * m_Velocity;
            if (m_Infection.enabled) {
                trailController.InitializeWith(m_InfectedColor, velocity, life);
            } else {
                trailController.InitializeWith(m_NormalColor, velocity, life);
            }
        }
    }
}
