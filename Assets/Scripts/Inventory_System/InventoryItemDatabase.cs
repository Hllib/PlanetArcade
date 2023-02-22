using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class InventoryItemDatabase : MonoBehaviour
{
    public List<InventoryItem> items = new List<InventoryItem>();

    void BuildDataBase()
    {
        items = new List<InventoryItem>()
        {
            new InventoryItem(InventoryTypes.Ammo, "Ammo", "You need it to use fire weapons",
            new Dictionary<string, int>
            {
                {"Bullets in one box", 15}
            }),

            new InventoryItem(InventoryTypes.Pistol, "Pistol", "OK weapon for starters",
            new Dictionary<string, int>
            {
                {"Damage", 5}
            }),

            new InventoryItem(InventoryTypes.Rifle, "Rifle", "Powerful weapon for a real space ranger",
            new Dictionary<string, int>
            {
                {"Damage", 10}
            }),

            new InventoryItem(InventoryTypes.Key, "Key", "A very little key will open a very heavy door",
            new Dictionary<string, int>
            {
                {"Chests or doors to unlock", 1}
            }),

            new InventoryItem(InventoryTypes.Gold, "Gold", "Jesper will be delighted",
            new Dictionary<string, int>
            {

            }),

            new InventoryItem(InventoryTypes.ArtifactMoon, "Moon Artifact", "This thing seems ancient....",
            new Dictionary<string, int>
            {

            }),

            new InventoryItem(InventoryTypes.ArtifactMars, "Mars Artifact", "",
            new Dictionary<string, int>
            {

            }),

            new InventoryItem(InventoryTypes.Shield, "Shield", "Protects the player from enemy attacks",
            new Dictionary<string, int>
            {
                {"Cooldown seconds", 7},
                {"Attacks deflected", 2}
            }),

            new InventoryItem(InventoryTypes.EnhancedShield, "Enhanced Shield", "Protects the player from enemy attacks",
            new Dictionary<string, int>
            {
                {"Cooldown seconds", 5},
                {"Attacks deflected", 3}
            }),

            new InventoryItem(InventoryTypes.Potion, "Potion", "Heals the player",
            new Dictionary<string, int>
            {
                {"Life units to heal", 4}
            })
        };
    }

    private void Awake()
    {
        BuildDataBase();
    }

    public InventoryItem FindItemById(int id)
    {
        return items.Find(item => item.id == id);
    }

    public InventoryItem FindItemByTitle(string title)
    {
        return items.Find(item => item.title == title);
    }
}
