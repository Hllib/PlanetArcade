using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetDatabase : MonoBehaviour
{
    private List<Planet> planetList;

    void BuildPlanetsDataBase()
    {
        planetList = new List<Planet>()
        {
            new Planet(PlanetID.Earth, "Earth", new Dictionary<string, string>
            {
                {"Population", "People, 8 billion"},
                {"Difficulty level", "Very easy" }
            }),

            new Planet(PlanetID.Moon, "Moon", new Dictionary<string, string>
            {
                {"Population", "Uninhabited"},
                {"Difficulty level", "Medium-Hard" }
            }),

            new Planet(PlanetID.Mars, "Mars", new Dictionary<string, string>
            {
                {"Population", "Uninhabited"},
                {"Difficulty level", "Very hard" }
            }),
            
            new Planet(PlanetID.Station3D, "Space Station", new Dictionary<string, string>
            {
                {"Population", "Operating personnel" }
            }),
        };
    }

    private void Awake()
    {
        BuildPlanetsDataBase();
    }

    public Planet FindPlanetById(int id)
    {
        return planetList.Find(planet => planet.id == id);
    }
}
