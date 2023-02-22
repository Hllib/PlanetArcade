using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlanetUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Planet _planet;
    private Tooltip _tooltip; 
    [SerializeField] private PlanetDatabase _planetDatabase;
    [SerializeField] private int _id;

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (_planet != null)
        {
            _tooltip.GenerateTooltipForPlanet(_planet);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _tooltip.gameObject.SetActive(false);
    }

    private void Start()
    {
        _tooltip = GameObject.Find("Tooltip").GetComponent<Tooltip>();
        if (_tooltip == null)
        {
            Debug.Log("tooltip is NULL::UIItem.cs");
        }
        AssignPlanet(_id);
    }

    public void AssignPlanet(int planetId)
    {
       _planet = _planetDatabase.FindPlanetById(planetId);
    }
}
