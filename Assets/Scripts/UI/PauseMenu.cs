using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private PlanetDatabase _planetDatabase;
    [SerializeField] private int _planetId;
    private Planet _planet;
    [SerializeField] private Text _planetInfoText;

    private void Start()
    {
        if (_planetDatabase != null)
        {
            AssignPlanet(_planetId);
            GeneratePlanetInfo(_planet);
        }
    }

    private void AssignPlanet(int planetId)
    {
        _planet = _planetDatabase.FindPlanetById(planetId);
    }

    private void GeneratePlanetInfo(Planet planet)
    {
        string descText = string.Empty;
        if (planet.description.Count > 0)
        {
            foreach (var desc in planet.description)
            {
                descText = desc.Key.ToString() + " : " + desc.Value.ToString() + "\n";
            }
        }

        string info = string.Format("<b>Location : {0}</b>\n{1}\n", planet.name, descText);

        _planetInfoText.text = info;
    }
}
