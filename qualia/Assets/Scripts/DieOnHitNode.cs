using UnityEngine;
using System.Collections;

public class DieOnHitNode : MonoBehaviour
{

    public string m_DeathSceneName;

    void ReachedNode (Node node)
    {
        Infected infection = node.GetComponent<Infected>();
        if (infection && infection.enabled)
        {
            Debug.Log(infection.enabled);
            Debug.Log("DIE!");
            Destroy(gameObject);
            Application.LoadLevel(m_DeathSceneName);
        }
    }
}
