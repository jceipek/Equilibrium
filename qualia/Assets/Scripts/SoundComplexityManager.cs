using UnityEngine;
using System.Collections;


public class SoundComplexityManager : MonoBehaviour {

    public int m_CurrentComplexity = 0;
    public AudioClip[] m_Mood1Audio;

    public float m_MaxVolume = 0.5f;
    private int m_AudioPlayerIndex = 0;
    private int m_ComplexityDirection = 1;
    private AudioSource[] m_AudioPlayers;

    void OnEnable () {
        m_AudioPlayers = new AudioSource[2];
        for (int i = 0; i < m_AudioPlayers.Length; i++)
        {
            m_AudioPlayers[i] = gameObject.AddComponent("AudioSource") as AudioSource;
            m_AudioPlayers[i].loop = true;
        }
    }

    private IEnumerator ChangeMusic ()
    {
        float maxTime = 3f;
        float timeCounter = 0f;
        float fractionComplete = 0f;
        while (!Mathf.Approximately(timeCounter, maxTime))
        {
            fractionComplete = timeCounter/maxTime;
            timeCounter = Mathf.Clamp(timeCounter + Time.deltaTime, 0, maxTime);
            m_AudioPlayers[m_AudioPlayerIndex].volume = Mathf.SmoothStep(0, m_MaxVolume, fractionComplete);
            m_AudioPlayers[GetNextAudioPlayerIndex()].volume = Mathf.SmoothStep(m_MaxVolume, 0, fractionComplete);
            yield return new WaitForSeconds(0.001f);
        }
        m_AudioPlayers[m_AudioPlayerIndex].volume = m_MaxVolume;
        m_AudioPlayers[GetNextAudioPlayerIndex()].volume = 0f;
        StopCoroutine("ChangeMusic");
    }

    private int GetNextAudioPlayerIndex ()
    {
        return (m_AudioPlayerIndex + 1) % 2;
    }

    public void IncreaseComplexity ()
    {
        /*if (m_CurrentComplexity > 0)
        {
            m_AudioPlayers[m_AudioPlayerIndex].clip = m_Mood1Audio[m_CurrentComplexity - 1];
        }
        else
        {
            m_AudioPlayers[m_AudioPlayerIndex].clip = null;
        }*/
        m_AudioPlayers[GetNextAudioPlayerIndex()].clip = m_Mood1Audio[m_CurrentComplexity];
        m_AudioPlayers[GetNextAudioPlayerIndex()].volume = 0f;
        m_AudioPlayers[m_AudioPlayerIndex].volume = m_MaxVolume;
        m_AudioPlayers[GetNextAudioPlayerIndex()].timeSamples = m_AudioPlayers[m_AudioPlayerIndex].timeSamples;
        m_AudioPlayers[GetNextAudioPlayerIndex()].Play();
        m_AudioPlayers[m_AudioPlayerIndex].Play();

        StartCoroutine("ChangeMusic");

        m_AudioPlayerIndex = GetNextAudioPlayerIndex();
        m_CurrentComplexity += m_ComplexityDirection;
        if (m_CurrentComplexity + m_ComplexityDirection == m_Mood1Audio.Length ||
            m_CurrentComplexity + m_ComplexityDirection == -1)
        {
            m_ComplexityDirection *= -1;
        }
        //m_CurrentComplexity %= m_Mood1Audio.Length;
    }
}
