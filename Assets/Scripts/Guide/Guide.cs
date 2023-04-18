using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Guide : MonoBehaviour
{
    [SerializeField]
    private GameObject _guidePanel;
    [SerializeField]
    private Text _guideText;
    [SerializeField]
    private GameObject _arrowPointer;
    [SerializeField]
    private GameObject _disabledAmmo;
    [SerializeField]
    private GameObject _slime;
    [SerializeField]
    private Text _okButtonText;
    [SerializeField]
    private Player _player;
    [SerializeField]
    private GameObject _scrollbar;

    public bool HasFinishedTutorial { get; set; }

    private static Guide _instance;

    public static Guide Instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.LogError("Guide is NULL! :: Guide.cs");
            }

            return _instance;
        }
    }

    private void Awake()
    {
        _instance = this;
    }

    public void EnableAmmo()
    {
        GenerateGuide(Script.OpenedChest);
        ShowPanel();

        _disabledAmmo.SetActive(true);
    }

    public void EnableSlime()
    {
        GenerateGuide(Script.FoundAmmo);
        ShowPanel();

        _slime.SetActive(true);
    }

    public void FinalMessage()
    {
        _okButtonText.text = "Finish level";
        GenerateGuide(Script.KilledSlime);
        ShowPanel();
    }

    public void GenerateGuide(string fileName)
    {
        _scrollbar.GetComponent<Scrollbar>().value = 1;
        string readFromFilePath = Application.streamingAssetsPath + "/Guides/" + fileName + ".txt";

        List<string> lines = File.ReadAllLines(readFromFilePath).ToList();
        StringBuilder text = new StringBuilder();
        foreach (string line in lines)
        {
            text.AppendLine(line);
        }
        _guideText.text = text.ToString();
    }

    public void ShowPanel()
    {
        if (_guidePanel.activeSelf == true) //closing the panel
        {
            _player.FireBlocked = false;
        }
        else //opening the panel
        {
            _player.FireBlocked = true;
        }

        _guidePanel.SetActive(!_guidePanel.activeSelf);
    }

    public void FinishGuide()
    {
        if (HasFinishedTutorial)
        {
            GameManager.Instance.levelFinished[PlanetID.Earth] = 1; // 1 means the level was finished
            GameManager.Instance.SavePlayerPrefs();
            GameManager.Instance.LoadScene("PlanetsMenu");
        }
    }

    public void CheckLevelFinish_Cave(bool hasKey)
    {
        ShowPanel();
        switch (hasKey)
        {
            case true: GenerateGuide(Script.LevelCaveHasKey); break;
            case false: GenerateGuide(Script.LevelCaveNoKey); break;
        }
    }

    public void PickUpShield()
    {
        ShowPanel();
        GenerateGuide(Script.PickUpShiled);
    }

    private bool _hasGivenKey;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Player>() != null && !_hasGivenKey)
        {
            _arrowPointer.gameObject.SetActive(false);
            GenerateGuide(Script.FirstMeet);
            ShowPanel();

            Inventory _playerInv = collision.GetComponent<Inventory>();
            _playerInv.GiveItem(InventoryTypes.Key);
            _hasGivenKey = true;
        }
    }
}
