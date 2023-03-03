using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StationManager : MonoBehaviour
{
    public bool HasPlayerVisited { get; set; }
    public bool HasPlayerTalkedToAll { get; set; }

    [SerializeField]
    private GameObject _cutsceneHolder;
    [SerializeField]
    private Transform _cameraSpot;
    [SerializeField]
    private Transform _camera;

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

        if (HasPlayerVisited)
        {
            _cutsceneHolder.SetActive(false);
            _camera = _cameraSpot;
        }
        else
        {
            _cutsceneHolder.SetActive(true);
        }
    }
}
