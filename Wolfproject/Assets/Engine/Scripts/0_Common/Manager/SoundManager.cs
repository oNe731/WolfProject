using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private Dictionary<string, AudioClip> m_bgm = new Dictionary<string, AudioClip>();
    private Dictionary<string, AudioClip> m_effect = new Dictionary<string, AudioClip>();

    private float m_bgmSound = 0.2f;
    private float m_effectSound = 0.5f;

    public float BgmSound { get => m_bgmSound; set => m_bgmSound = value; }
    public float EffectSound { get => m_effectSound; set => m_effectSound = value; }

    private void Start()
    {
        Load_Resource();
    }

    public void Update_AllAudioSources()
    {
        AudioSource[] allAudioSources = FindObjectsOfType<AudioSource>();
        foreach (var audioSource in allAudioSources)
        {
            if (audioSource.gameObject.CompareTag("MainCamera") == true)
                audioSource.volume = m_bgmSound;
            else
                audioSource.volume = m_effectSound;
        }
    }

    private void Load_Resource()
    {
        //*
    }

    public void Play_ManagerAudioSource(string name, bool loop, float speed)
    {
        AudioSource audioSource = GameManager.Ins.AudioSource;
        Play_AudioSource(audioSource, name, loop, speed);
    }

    public void Play_AudioSource(AudioSource audioSource, string name, bool loop, float speed)
    {
        Play_AudioSource(audioSource, m_effect[name], loop, speed, m_effectSound);
    }

    public void Play_AudioSourceBGM(string name, bool loop, float speed)
    {
        Play_AudioSource(Camera.main.GetComponent<AudioSource>(), m_bgm[name], loop, speed, m_bgmSound);
    }

    public void Stop_AudioSourceBGM()
    {
        Camera.main.GetComponent<AudioSource>().Stop();
    }

    public void Play_AudioSource(AudioSource audioSource, AudioClip audioClip, bool loop, float speed, float volume)
    {
        audioSource.Stop();

        audioSource.clip = audioClip;
        audioSource.loop = loop;
        audioSource.pitch = speed; // ±âº»1f
        audioSource.volume = volume;
        audioSource.Play();
    }

    public AudioClip Get_EffectAudioClip(string name)
    {
        return m_effect[name];
    }

    public void Stop_AudioSource(AudioSource audioSource)
    {
        audioSource.Stop();
    }
}
