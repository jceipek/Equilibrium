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
    }

    public void PlayConnectionSound ()
    {
        m_AudioSource.clip = m_NewConnectionSound;
        m_AudioSource.Play();
    }

}
