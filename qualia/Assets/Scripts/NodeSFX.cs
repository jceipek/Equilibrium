using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(AudioSource))]
public class NodeSFX : MonoBehaviour
{
    public AudioClip[] m_NewConnectionSound;
    public AudioClip[] m_NewInfectedConnectionSound;
    public AudioClip m_InfectionSound;

    private AudioSource m_AudioSource;

    void OnEnable ()
    {
        m_AudioSource = GetComponent<AudioSource>();

        //WWW www = new WWW("file://"+System.IO.Path.Combine(Application.streamingAssetsPath, "new-connection.ogg"));
        //m_NewConnectionSound = www.GetAudioClip(true);
    }

    public void PlayConnectionSound (Node node1, Node node2)
    {
        AudioClip sound;
        Infected infection1 = node1.GetComponent<Infected>();
        Infected infection2 = node2.GetComponent<Infected>();
        if (infection1.enabled || infection2.enabled)
        {
            sound = m_NewInfectedConnectionSound[Random.Range(0, m_NewInfectedConnectionSound.Length)];
        }
        else
        {
            sound = m_NewConnectionSound[Random.Range(0, m_NewConnectionSound.Length)];
        }

        AudioSource.PlayClipAtPoint(sound, gameObject.transform.position, 1f);
    }

    public void PlayLoopedInfectionSound ()
    {
        m_AudioSource.loop = true;
        m_AudioSource.clip = m_InfectionSound;
        m_AudioSource.Play();
    }

}
