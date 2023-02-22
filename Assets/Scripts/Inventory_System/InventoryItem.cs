using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryItem
{
    public bool isSelected { get; set; }

    public int id;
    public string title;
    public string description;
    public Sprite icon;
    public Dictionary<string, int> stats = new Dictionary<string, int>();

    public InventoryItem(int id, string title, string description,
        Dictionary<string, int> stats)
    {
        this.id = id;
        this.title = title;
        this.description = description;
        this.icon = Resources.Load<Sprite>("Sprites/Items/" + title);
        this.stats = stats;
    }

    public InventoryItem(InventoryItem item)
    {
        this.id = item.id;
        this.title = item.title;
        this.description = item.description;
        this.icon = Resources.Load<Sprite>("Sprites/Items/" + item.title);
        this.stats = item.stats;
    }

}
