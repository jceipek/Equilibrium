using UnityEngine;
using System.Collections;


public class SoundComplexityManager : MonoBehaviour {

	public int m_CurrentComplexity = 0;
	public AudioClip[] m_Mood1Audio;

	private AudioSource m_CurrentAudioPlayer;
	private AudioSource m_NextAudioPlayer;

	void OnEnable () {
		m_CurrentAudioPlayer = gameObject.AddComponent("AudioSource") as AudioSource;
		m_CurrentAudioPlayer.loop = true;
		m_NextAudioPlayer = gameObject.AddComponent("AudioSource") as AudioSource;
		m_NextAudioPlayer.loop = true;
	}

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {

	}

	public void IncreaseComplexity ()
	{
		m_CurrentAudioPlayer.clip = m_Mood1Audio[m_CurrentComplexity];
		m_CurrentAudioPlayer.Play();
		m_CurrentComplexity++;
		m_CurrentComplexity %= m_Mood1Audio.Length;
	}
}
