using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StationManager : MonoBehaviour
{
    public bool HasPlayerVisited { get; set; }
    public bool HasPlayerTalkedToAll { get; set; }

    [SerializeField]
    private GameObject _introCutsceneHolder;
    [SerializeField]
    private GameObject _outroCutsceneHolder;
    [SerializeField]
    private Transform _cameraSpot;
    [SerializeField]
    private Transform _camera;
    [SerializeField]
    private Player3D _player;
    [SerializeField]
    public bool PlayerFinishedFinalDialoge { get; set; }

    private static StationManager _instance;

    public static StationManager Instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.LogError("StationManager is NULL! :: StationManager.cs");
            }

            return _instance;
        }
    }

    private void Awake()
    {
        _instance = this;
        HasPlayerVisited = PlayerPrefs.GetInt(PlayerSettings.Station, 0) == 1 ? true : false;
        HasPlayerTalkedToAll = HasPlayerVisited;
        GameManager.Instance.CheckGameFinish();
    }

    private void Start()
    {
        if (HasPlayerVisited)
        {
            _introCutsceneHolder.SetActive(false);
            _camera.position = _cameraSpot.position;
            _camera.rotation = _cameraSpot.rotation;

            AudioManager.Instance.StopMusic();
            AudioManager.Instance.InitMusic(FMODEvents.Instance.musicOptional);
        }
        else
        {
            _introCutsceneHolder.SetActive(true);
            BlockMovement();
        }
    }

    //called from timeLine through a signal
    public void UnblockMovement()
    {
        _player.BlockMovement = false;
    }

    public void BlockMovement()
    {
       _player.BlockMovement = true;
    }

    private void Update()
    {
        if (PlayerFinishedFinalDialoge)
        {
            _outroCutsceneHolder.SetActive(true);
        }
    }

    public void StartVictoryMusic()
    {
        AudioManager.Instance.StopMusic();
        AudioManager.Instance.InitMusic(FMODEvents.Instance.finalMusic);
    }

    public void LoadMenuAfterEnd()
    {
        PlayerPrefs.SetInt(PlayerSettings.ShowAchievement, PlayerSettings.Done);
        GameManager.Instance.LoadScene("MainMenu");
    }
}
