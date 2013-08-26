using UnityEngine;
using System.Collections;

public class DieOnHitNode : MonoBehaviour
{

    public string m_DeathSceneName;
    public AudioClip m_DeathSound;

    private AudioSource m_AudioSource;
    private AvatarController m_Avatar;

    void OnEnable ()
    {
        m_Avatar = gameObject.GetComponent<AvatarController>();
        m_AudioSource = gameObject.GetComponent<AudioSource>();
    }

    void ReachedNode (Node node)
    {
        Infected infection = node.GetComponent<Infected>();
        if (infection && infection.enabled && m_Avatar.m_LevelTransitioning == false)
        {
            AudioSource.PlayClipAtPoint(m_DeathSound, gameObject.transform.position, 1.0f);
            m_AudioSource.PlayOneShot(m_DeathSound, 10.0f);
            Debug.Log("DIE!");
            m_Avatar.TransitionToLevel(m_DeathSceneName);
        }
    }
}
