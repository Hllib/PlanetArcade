using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetMenuCanvas : MonoBehaviour
{
    [SerializeField]
    private GameObject[] _checkMarks;

    void Start()
    {
        switch(PlayerPrefs.GetInt("Earth"))
        {
            case 0: _checkMarks[PlanetID.Earth].SetActive(false); break;
            case 1: _checkMarks[PlanetID.Earth].SetActive(true); break;
        }
        switch (PlayerPrefs.GetInt("Moon"))
        {
            case 0: _checkMarks[PlanetID.Moon].SetActive(false); break;
            case 1: _checkMarks[PlanetID.Moon].SetActive(true); break;
        }
        switch (PlayerPrefs.GetInt("Mars"))
        {
            case 0: _checkMarks[PlanetID.Mars].SetActive(false); break;
            case 1: _checkMarks[PlanetID.Mars].SetActive(true); break;
        }
    }
}
