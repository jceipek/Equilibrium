using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(AudioSource))]
public class NodeSFX : MonoBehaviour
{
    public AudioClip m_NewConnectionSound;

    private AudioSource m_AudioSource;

    void OnEnable ()
    {
        m_AudioSource = GetComponent<AudioSource>();

        WWW www = new WWW("file://"+System.IO.Path.Combine(Application.streamingAssetsPath, "new-connection.ogg"));
        m_NewConnectionSound = www.GetAudioClip(true);
    }

    public void PlayConnectionSound ()
    {
        m_AudioSource.clip = m_NewConnectionSound;
        m_AudioSource.Play();
    }

}
