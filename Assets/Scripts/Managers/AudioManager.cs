using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private static AudioManager _instance;
    private AudioSource _audioSource;

    [SerializeField]
    private AudioClip _firePistol;
    [SerializeField]
    private AudioClip _fireRifle;
    [SerializeField]
    private AudioClip _bgMusic_calm;
    [SerializeField]
    private AudioClip _bgMusic_epic;
    [SerializeField]
    private AudioSource _bgMusicSource;

    public static AudioManager Instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.LogError("Audio Manager is NULL! :: AudioManager.cs");
            }

            return _instance;
        }
    }

    private void Awake()
    {
        _instance = this;
        _audioSource = GetComponent<AudioSource>();
    }

    public void BgMusic_Calm()
    {
        _bgMusicSource.clip = _bgMusic_calm;
        _bgMusicSource.Play();
    }

    public void BgMusic_Epic()
    {
        _bgMusicSource.clip = _bgMusic_epic;
        _bgMusicSource.Play();
    }
}
