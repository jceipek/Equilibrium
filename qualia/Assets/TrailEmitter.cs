using UnityEngine;
using System.Collections;

public class TrailEmitter : MonoBehaviour {

    public float m_Velocity;
    public float m_RefreshFrequency;

	public Node m_Node;
	private Infected m_Infection;

    void OnEnable ()
    {
    	m_Infection = m_Node.GetComponentInChildren<Infected>();
    }

    void Start ()
    {
        StartCoroutine(EmissionTimer());
    }

	IEnumerator EmissionTimer ()
	{
        yield return new WaitForSeconds(m_RefreshFrequency);
        Emit();
        StartCoroutine(EmissionTimer());
    }

	private void Emit ()
	{
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
                trailController.InitializeWith(Constants.g.TRAIL_INFECTED_TINT_COLOR, velocity, life);
            } else {
                trailController.InitializeWith(Constants.g.TRAIL_NORMAL_TINT_COLOR, velocity, life);
            }
        }
    }
}
