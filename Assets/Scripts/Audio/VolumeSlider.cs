using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour
{
    private enum VolumeType
    {
        Master,
        Music,
        Ambience,
        SFX
    }

    [SerializeField]
    private VolumeType volumeType;

    private Slider volumeSlider;

    private void Awake()
    {
        volumeSlider = GetComponent<Slider>();
    }

    private void Start()
    {
        AudioManager.Instance.masterVolume = PlayerPrefs.GetFloat(PlayerSettings.MasterVolume, 1);
        AudioManager.Instance.musicVolume = PlayerPrefs.GetFloat(PlayerSettings.MusicVolume, 1);
        AudioManager.Instance.ambienceVolume = PlayerPrefs.GetFloat(PlayerSettings.AmbienceVolume, 1);
        AudioManager.Instance.sfxVolume = PlayerPrefs.GetFloat(PlayerSettings.SFXVolume, 1);
    }

    private void Update()
    {
        switch (volumeType)
        {
            case VolumeType.Master:
                volumeSlider.value = AudioManager.Instance.masterVolume;
                break;
            case VolumeType.Music:
                volumeSlider.value = AudioManager.Instance.musicVolume;
                break;
            case VolumeType.Ambience:
                volumeSlider.value = AudioManager.Instance.ambienceVolume;
                break;
            case VolumeType.SFX:
                volumeSlider.value = AudioManager.Instance.sfxVolume;
                break;
            default: Debug.Log("Unexpected volume type!"); break;
        }
    }

    public void OnSliderValueChanged()
    {
        switch (volumeType)
        {
            case VolumeType.Master:
                AudioManager.Instance.masterVolume = volumeSlider.value;
                PlayerPrefs.SetFloat(PlayerSettings.MasterVolume, volumeSlider.value);
                break;
            case VolumeType.Music:
                AudioManager.Instance.musicVolume = volumeSlider.value;
                PlayerPrefs.SetFloat(PlayerSettings.MusicVolume, volumeSlider.value);
                break;
            case VolumeType.Ambience:
                AudioManager.Instance.ambienceVolume = volumeSlider.value;
                PlayerPrefs.SetFloat(PlayerSettings.AmbienceVolume, volumeSlider.value);
                break;
            case VolumeType.SFX:
                AudioManager.Instance.sfxVolume = volumeSlider.value;
                PlayerPrefs.SetFloat(PlayerSettings.SFXVolume, volumeSlider.value);
                break;
            default: Debug.Log("Unexpected volume type!"); break;
        }
    }
}
