using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet
{
    public int id;
    public string name;
    public Dictionary<string, string> description = new Dictionary<string, string>();

    public Planet(int id, string planetName, Dictionary<string, string> description)
    {
        this.id = id;
        this.description = description;
        this.name = planetName;
    }
}
