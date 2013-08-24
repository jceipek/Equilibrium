using UnityEngine;
using System.Collections;

public class DieOnHitNode : MonoBehaviour
{

    public string m_DeathSceneName;

    private AvatarController m_Avatar;

    void OnEnable ()
    {
        m_Avatar = gameObject.GetComponent<AvatarController>();
    }

    void ReachedNode (Node node)
    {
        Infected infection = node.GetComponent<Infected>();
        if (infection && infection.enabled && m_Avatar.m_LevelTransitioning == false)
        {
            Debug.Log("DIE!");
            m_Avatar.TransitionToLevel(m_DeathSceneName);
        }
    }
}
