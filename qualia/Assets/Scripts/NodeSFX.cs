using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(AudioSource))]
public class NodeSFX : MonoBehaviour
{
    public AudioClip m_NewConnectionSound;
    public AudioClip m_InfectionSound;

    private AudioSource m_AudioSource;

    void OnEnable ()
    {
        m_AudioSource = GetComponent<AudioSource>();

        WWW www = new WWW("file://"+System.IO.Path.Combine(Application.streamingAssetsPath, "new-connection.ogg"));
        m_NewConnectionSound = www.GetAudioClip(true);
    }

    public void PlayConnectionSound ()
    {
        AudioSource.PlayClipAtPoint(m_NewConnectionSound, gameObject.transform.position, 0.5f);
    }

    public void PlayLoopedInfectionSound ()
    {
        m_AudioSource.loop = true;
        m_AudioSource.clip = m_InfectionSound;
        m_AudioSource.Play();
    }

}
