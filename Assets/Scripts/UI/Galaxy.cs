using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Galaxy : MonoBehaviour
{
    [SerializeField]
    private PlanetDatabase _planetDatabase;

    public List<Planet> PlanetList = new List<Planet>();

    public void AssignPlanet(int planetId)
    {
        Planet planetToAssign = _planetDatabase.FindPlanetById(planetId);
        PlanetList.Add(planetToAssign);
    }
}
