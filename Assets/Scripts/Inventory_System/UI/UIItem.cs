using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Runtime.CompilerServices;

public class UIItem : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public InventoryItem item;
    private Image spriteImage;
    private UIItem _selectedItem;
    private Tooltip _tooltip;
    private Player _player;

    private void Awake()
    {
        spriteImage = GetComponent<Image>();

        if (spriteImage == null)
        {
            Debug.Log("Image is NULL::UIItem.cs");
        }
        UpdateItem(null);

        _selectedItem = GameObject.Find("SelectedItem").GetComponent<UIItem>();
        if (_selectedItem == null)
        {
            Debug.Log("SelectedItem is NULL::UIItem.cs");
        }

        _tooltip = GameObject.Find("Tooltip").GetComponent<Tooltip>();
        if (_tooltip == null)
        {
            Debug.Log("tooltip is NULL::UIItem.cs");
        }

        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    public void UpdateItem(InventoryItem item)
    {
        this.item = item;

        if (this.item != null)
        {
            spriteImage.color = Color.white;
            spriteImage.sprite = this.item.icon;
        }
        else
        {
            spriteImage.color = Color.clear;
        }

    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (this.item != null)
        {
            AudioManager.Instance.PlayOneShot(FMODEvents.Instance.itemInteraction, Vector3.zero);
            if (_selectedItem.item != null)
            {
                InventoryItem clone = new InventoryItem(_selectedItem.item);
                _selectedItem.UpdateItem(this.item);

                UpdateItem(clone);
            }
            else
            {
                _selectedItem.UpdateItem(this.item);
                UpdateItem(null);
            }
        }
        else if (_selectedItem.item != null)
        {
            AudioManager.Instance.PlayOneShot(FMODEvents.Instance.itemInteraction, Vector3.zero);
            UpdateItem(_selectedItem.item);
            _selectedItem.UpdateItem(null);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(_player!= null)
        {
            if (this.item != null)
            {
                _tooltip.GenerateTooltip(this.item);
            }
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (_player != null)
        {
            _tooltip.gameObject.SetActive(false);
        }
    }
}
