using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Linq;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private PlanetDatabase _planetDatabase;
    [SerializeField] private int _planetId;
    private Planet _planet;
    [SerializeField] private Text _planetInfoText;
    [SerializeField] private Text _levelDescription;
    [SerializeField] private string _sceneName;

    private void Start()
    {
        if (_planetDatabase != null)
        {
            AssignPlanet(_planetId);
            if (_planetInfoText != null)
            {
                GeneratePlanetInfo(_planet);
            }
            if (_levelDescription != null)
            {
                GenerateLevelDesc(_sceneName);
            }
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

    public void GenerateLevelDesc(string fileName)
    {
        string readFromFilePath = Application.streamingAssetsPath + "/Levels/" + fileName + ".txt";

        List<string> lines = File.ReadAllLines(readFromFilePath).ToList();
        StringBuilder text = new StringBuilder();
        foreach (string line in lines)
        {
            text.AppendLine(line);
        }

        _levelDescription.text = text.ToString();
    }
}
