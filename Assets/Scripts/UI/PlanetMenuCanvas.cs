using Newtonsoft.Json.Bson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetMenuCanvas : MonoBehaviour
{
    [SerializeField]
    private GameObject _soundPanel;
    [SerializeField]
    private GameObject[] _checkMarks;
    [SerializeField]
    private GameObject[] _pointers;

    private Animator _animator;

    void Start()
    {
        _animator = GetComponent<Animator>();
        _soundPanel.SetActive(false);

        SetCheckMarks();
        ChooseCurrentHint();
    }

    public void ShowSoundPanel()
    {
        _soundPanel.SetActive(!_soundPanel.activeSelf);
    }

    private void SetCheckMarks()
    {
        switch (PlayerPrefs.GetInt(PlayerSettings.Earth))
        {
            case 0: _checkMarks[PlanetID.Earth].SetActive(false); break;
            case 1: _checkMarks[PlanetID.Earth].SetActive(true); break;
        }
        switch (PlayerPrefs.GetInt(PlayerSettings.Moon))
        {
            case 0: _checkMarks[PlanetID.Moon].SetActive(false); break;
            case 1: _checkMarks[PlanetID.Moon].SetActive(true); break;
        }
        switch (PlayerPrefs.GetInt(PlayerSettings.Mars))
        {
            case 0: _checkMarks[PlanetID.Mars].SetActive(false); break;
            case 1: _checkMarks[PlanetID.Mars].SetActive(true); break;
        }
    }

    private void ChooseCurrentHint()
    {
        int planetIdToChooseAsHint = PlayerPrefs.GetInt(PlayerSettings.Earth) == PlayerSettings.NewGame ? PlanetID.Earth
            : PlayerPrefs.GetInt(PlayerSettings.Earth) == PlayerSettings.LevelFinished
            && PlayerPrefs.GetInt(PlayerSettings.Moon) == PlayerSettings.NewGame ? PlanetID.Moon
            : PlanetID.Mars;

        switch (planetIdToChooseAsHint)
        {
            case PlanetID.Earth:
                _pointers[PlanetID.Earth].SetActive(true);
                _animator.SetInteger("PlanetID", PlanetID.Earth);
                break;
            case PlanetID.Moon:
                _pointers[PlanetID.Moon].SetActive(true);
                _animator.SetInteger("PlanetID", PlanetID.Moon);
                break;
            case PlanetID.Mars:
                _pointers[PlanetID.Mars].SetActive(true);
                _animator.SetInteger("PlanetID", PlanetID.Mars);
                break;
        }
    }
}
