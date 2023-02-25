using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIInventory : MonoBehaviour
{
    public List<UIItem> uiItems = new List<UIItem>();
    public GameObject slotPrefab;
    public Transform slotPanel;

    public int numberOfSlots;

    [SerializeField]
    private GameObject[] quickSelectorSlots;

    private void Awake()
    {
        for (int i = 0; i < numberOfSlots; i++)
        {
            GameObject instance = Instantiate(slotPrefab);
            instance.transform.SetParent(slotPanel, false);

            uiItems.Add(instance.GetComponentInChildren<UIItem>());
        }
    }

    public void UpdateSlot(int slot, InventoryItem item)
    {
        uiItems[slot].UpdateItem(item);
    }

    public void AddNewItem(InventoryItem item)
    {
        UpdateSlot(uiItems.FindIndex(i => i.item == null), item);
    }

    public void RemoveItem(InventoryItem item)
    {
        int indexOfItem = uiItems.FindIndex(i => i.item == item);

        if (indexOfItem == -1)
        {
            Debug.Log("Deleting item from quick selector!");
            for (int i = 0; i < quickSelectorSlots.Length; i++)
            {
                if (quickSelectorSlots[i].GetComponentInChildren<UIItem>().item == item)
                {
                    quickSelectorSlots[i].GetComponentInChildren<UIItem>().item = null;
                    quickSelectorSlots[i].GetComponentInChildren<UIItem>().GetComponent<Image>().color = Color.clear;
                    break;
                }
            }
        }
        else
        {
            UpdateSlot(indexOfItem, null);
        }
    }
}
