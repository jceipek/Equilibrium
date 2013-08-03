// XXX (Julian): This component has bad strong coupling.
// It doesn't even make sense anymore.

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent (typeof (Collider))]
public class Pickable : MonoBehaviour {

	private GameObject m_node;
	public List<GameObject> m_connected_to = new List<GameObject>();

	void OnEnable () {
		// XXX (Julian): Is this the right place to put this?
		m_node = transform.parent.gameObject;
	}

	void Update () {

	}

	public void ConnectTo (Pickable obj) {
		//gameObject.SendMessage("Respawn", SendMessageOptions.DontRequireReceiver);
		m_connected_to.Add(obj.gameObject);
	}

	void OnDrawGizmos () {
		foreach (GameObject obj in m_connected_to) {
			Gizmos.DrawLine(transform.position, obj.transform.position);
		}
	}
}