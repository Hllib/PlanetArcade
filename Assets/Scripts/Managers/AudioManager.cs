using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private static AudioManager _instance;
    private AudioSource _audioSource;

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

    public void PlayClip(AudioClip clip)
    {
        _audioSource.clip = clip;
        _audioSource.Play();
    }
}
