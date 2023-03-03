using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _pauseMenu;
    [SerializeField]
    private Animator _rocketAnimator;
    [SerializeField]
    private Player _player;
    [SerializeField]
    private Player3D _player3D;

    public bool IsPlayerDead { get; set; }
    public bool IsPaused { get; private set; }

    private static GameManager _instance;

    public Dictionary<int, int> levelFinished = new Dictionary<int, int>();

    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.LogError("Game Manager is NULL::GameManager.cs");
            }

            return _instance;
        }
    }

    private void Awake()
    {
        _instance = this;
        CheckPauseState();

        levelFinished = new Dictionary<int, int>()
        {
            {PlanetID.Earth, PlayerPrefs.GetInt(PlayerSettings.Earth, 0)},
            {PlanetID.Moon, PlayerPrefs.GetInt(PlayerSettings.Moon, 0)},
            {PlanetID.Mars, PlayerPrefs.GetInt(PlayerSettings.Mars, 0)}
        };
    }

    public void SavePlayerPrefs()
    {
        PlayerPrefs.SetInt(PlayerSettings.Earth, levelFinished[PlanetID.Earth]);
        PlayerPrefs.SetInt(PlayerSettings.Moon, levelFinished[PlanetID.Moon]);
        PlayerPrefs.SetInt(PlayerSettings.Mars, levelFinished[PlanetID.Mars]);
        PlayerPrefs.Save();
        _player.SaveInventory();
    }

    public void LoadScene(string sceneName)
    {
        StopPause();
        SceneManager.LoadScene(sceneName);
    }

    public void CheckLocationAccessibility(int id)
    {
        bool canGo = false;
        switch (id)
        {
            case PlanetID.Earth:
                canGo = PlayerPrefs.GetInt(PlayerSettings.Earth) == PlayerSettings.NewGame ? true : false;
                break;
            case PlanetID.Moon:
                canGo = PlayerPrefs.GetInt(PlayerSettings.Moon) == PlayerSettings.NewGame
                    && PlayerPrefs.GetInt(PlayerSettings.Earth) == PlayerSettings.LevelFinished ?
                    true : false;
                break;
            case PlanetID.Mars:
                canGo = PlayerPrefs.GetInt(PlayerSettings.Mars) == PlayerSettings.NewGame
                    && PlayerPrefs.GetInt(PlayerSettings.Earth) == PlayerSettings.LevelFinished
                    && PlayerPrefs.GetInt(PlayerSettings.Moon) == PlayerSettings.LevelFinished ?
                    true : false;
                break;
            case PlanetID.Station3D: canGo = true; break;
            default: canGo = false; break;
        }

        switch (canGo)
        {
            case true: GoToLocation(id); break;
            case false: Debug.Log("You've already been there or you've not finished previous levels!"); break;
        }
    }

    public void GoToLocation(int id)
    {
        if (_rocketAnimator != null)
        {
            switch (id)
            {
                case PlanetID.Earth:
                    _rocketAnimator.SetInteger("PlanetID", 0);
                    StartCoroutine(LoadSceneWithDelay(PlanetID.Earth));
                    break;
                case PlanetID.Moon:
                    _rocketAnimator.SetInteger("PlanetID", 1);
                    StartCoroutine(LoadSceneWithDelay(PlanetID.Moon));
                    break;
                case PlanetID.Mars:
                    _rocketAnimator.SetInteger("PlanetID", 2);
                    StartCoroutine(LoadSceneWithDelay(PlanetID.Mars));
                    break;
                case PlanetID.Station3D:
                    _rocketAnimator.SetInteger("PlanetID", 3);
                    StartCoroutine(LoadSceneWithDelay(PlanetID.Station3D));
                    break;
            }
        }
    }

    IEnumerator LoadSceneWithDelay(int id)
    {
        AudioManager.Instance.PlayOneShot(FMODEvents.Instance.rocketFly, Vector3.zero);
        StopPause();
        yield return new WaitForSeconds(2f);

        switch (id)
        {
            case PlanetID.Earth: LoadScene("Earth"); break;
            case PlanetID.Moon: LoadScene("Moon"); break;
            case PlanetID.Mars: LoadScene("Mars"); break;
            case PlanetID.Station3D: LoadScene("Station3D"); break;
        }
    }

    private void StopPause()
    {
        Time.timeScale = 1f;
        IsPaused = false;

        if (_player3D != null)
        {
            _player3D.BlockMovement = false;
        }
    }

    private void StartPause()
    {
        if (_player3D != null)
        {
            _player3D.BlockMovement = true;
        }
        Time.timeScale = 0f;
        IsPaused = true;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && _pauseMenu != null && IsPlayerDead == false)
        {
            _pauseMenu.SetActive(!_pauseMenu.activeSelf);
            CheckPauseState();
            AudioManager.Instance.PlayOneShot(FMODEvents.Instance.buttonClick, Vector3.zero);
        }

        if (IsPlayerDead == true)
        {
            UIManager.Instance.ShowGameOverScren();
        }
    }

    public void FinishMoon()
    {
        GameManager.Instance.levelFinished[PlanetID.Moon] = PlayerSettings.LevelFinished;
        GameManager.Instance.SavePlayerPrefs();
        GameManager.Instance.LoadScene("PlanetsMenu");
    }

    public void MoveToFinalBoss()
    {
        GameManager.Instance.SavePlayerPrefs();
        GameManager.Instance.LoadScene("BossFight");
    }

    public void Resume()
    {
        _pauseMenu.SetActive(!_pauseMenu.activeSelf);
        CheckPauseState();
    }

    public void CheckPauseState()
    {
        if (_pauseMenu != null)
        {
            switch (_pauseMenu.activeSelf)
            {
                case true:
                    StartPause();
                    break;
                case false:
                    StopPause();
                    break;
            }
        }
    }
}
