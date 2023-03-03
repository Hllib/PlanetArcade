using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarsManager : MonoBehaviour
{
    [SerializeField]
    private Player _player;
    [SerializeField]
    private bool _isOnMars;

    private static MarsManager _instance;

    public static MarsManager Instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.LogError("Mars Manager is NULL! :: MarsManager.cs");
            }

            return _instance;
        }
    }

    private void Awake()
    {
        _instance = this;
    }

    void Start()
    {
        if (_isOnMars)
        {
            StartCoroutine(SaveInv());
        }
    }

    IEnumerator SaveInv()
    {
        yield return new WaitForSeconds(0.1f);

        //saving current inv and writing it to separate playerprefs string
        _player.SaveInventory();
        PlayerPrefs.SetString(PlayerSettings.WhenOnMarsInventory, PlayerPrefs.GetString(PlayerSettings.Inventory));
    }

    public void SetInventoryToMarsState()
    {
        PlayerPrefs.SetString(PlayerSettings.Inventory, PlayerPrefs.GetString(PlayerSettings.WhenOnMarsInventory));
    }

    public void StopFightMusic()
    {
        AudioManager.Instance.StopMusic();
        AudioManager.Instance.InitMusic(FMODEvents.Instance.winMusicBossFight);
    }
}
