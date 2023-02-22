using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tooltip : MonoBehaviour
{
    private Text _tooltipText;

    private void Start()
    {
        _tooltipText = GetComponentInChildren<Text>();
        if (_tooltipText == null)
        {
            Debug.Log("Tooltip is NULL::Tooltip.cs");
        }
        gameObject.SetActive(false);
    }

    public void GenerateTooltip(InventoryItem item)
    {
        string statText = "";
        if (item.stats.Count > 0)
        {
            foreach (var stat in item.stats)
            {
                statText += stat.Key.ToString() + ":" + stat.Value.ToString() + "\n";
            }
        }

        string tooltip = string.Format("<b>{0}</b>\n{1}\n\n<b>{2}</b>", item.title, item.description, statText);

        _tooltipText.text = tooltip;
        gameObject.SetActive(true);
    }

    public void GenerateTooltipForPlanet(Planet planet)
    {
        string descText = string.Empty;
        if (planet.description.Count > 0)
        {
            foreach (var desc in planet.description)
            {
                descText = desc.Key.ToString() + " : " + desc.Value.ToString() + "\n";
            }
        }

        string tooltip = string.Format("<b>{0}</b>\n<b>{1}</b>", planet.name, descText);
        _tooltipText.text = tooltip;
        gameObject.SetActive(true);
    }
}
