using System.Collections;
using System.Collections.Generic;
using System.Text;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public List<InventoryItem> playerItems = new List<InventoryItem>();

    [SerializeField]
    private InventoryItemDatabase _inventoryItemDatabase;
    [SerializeField]
    private UIInventory _uiInventory;
    [SerializeField]
    private GameObject[] _quickSelections;
    [SerializeField]
    private GameObject _scrollBar;

    [SerializeField]
    public InventoryItem SelectedItem { get; private set; }

    public void GiveItem(int itemId)
    {
        InventoryItem itemToAdd = _inventoryItemDatabase.FindItemById(itemId);
        playerItems.Add(itemToAdd);
        _uiInventory.AddNewItem(itemToAdd);

        if(UIManager.Instance != null)
            UIManager.Instance.DisplayMessage("Item added: " + itemToAdd.title);

        AudioManager.Instance.PlayOneShot(FMODEvents.Instance.itemInteraction, Vector3.zero);
    }

    public void GiveItem(string itemTitle)
    {
        InventoryItem itemToAdd = _inventoryItemDatabase.FindItemByTitle(itemTitle);
        playerItems.Add(itemToAdd);
        _uiInventory.AddNewItem(itemToAdd);

        if (UIManager.Instance != null)
            UIManager.Instance.DisplayMessage("Item added: " + itemToAdd.title);

        AudioManager.Instance.PlayOneShot(FMODEvents.Instance.itemInteraction, Vector3.zero);
    }

    public InventoryItem CheckForItem(int itemId)
    {
        return playerItems.Find(item => item.id == itemId);
    }

    public InventoryItem CheckForItem(string itemTitle)
    {
        return playerItems.Find(item => item.title == itemTitle);
    }

    public void RemoveItem(int itemId)
    {
        InventoryItem itemToRemove = CheckForItem(itemId);

        if (itemToRemove != null)
        {
            playerItems.Remove(itemToRemove);
            _uiInventory.RemoveItem(itemToRemove);
            if (UIManager.Instance != null)
                UIManager.Instance.DisplayMessage("Removed item: " + itemToRemove.title);
        }
    }

    public void RemoveItem(string itemTitle)
    {
        InventoryItem itemToRemove = CheckForItem(itemTitle);

        if (itemToRemove != null)
        {
            playerItems.Remove(itemToRemove);
            _uiInventory.RemoveItem(itemToRemove);
            if (UIManager.Instance != null)
                UIManager.Instance.DisplayMessage("Removed item: " + itemToRemove.title);
        }
    }

    private void Start()
    {
        if (_uiInventory == null)
        {
            Debug.Log("uiInventory is NULL::Inventory.cs");
        }
        if (_inventoryItemDatabase == null)
        {
            Debug.Log("inventoryItemDatabase is NULL::Inventory.cs");
        }

        _uiInventory.gameObject.SetActive(false);
        _scrollBar.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            _uiInventory.gameObject.SetActive(!_uiInventory.gameObject.activeSelf);
            _scrollBar.gameObject.SetActive(!_scrollBar.gameObject.activeSelf);
            AudioManager.Instance.PlayOneShot(FMODEvents.Instance.buttonClick, Vector3.zero);
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (_quickSelections[0].GetComponentInChildren<UIItem>().item != null)
            {
                SelectedItem = _quickSelections[0].GetComponentInChildren<UIItem>().item;
                UIManager.Instance.DisplayMessage("Player holds " + SelectedItem.title);
                AudioManager.Instance.PlayOneShot(FMODEvents.Instance.itemEquip, Vector3.zero);
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            if (_quickSelections[1].GetComponentInChildren<UIItem>().item != null)
            {
                SelectedItem = _quickSelections[1].GetComponentInChildren<UIItem>().item;
                UIManager.Instance.DisplayMessage("Player holds " + SelectedItem.title);
                AudioManager.Instance.PlayOneShot(FMODEvents.Instance.itemEquip, Vector3.zero);
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            if (_quickSelections[2].GetComponentInChildren<UIItem>().item != null)
            {
                SelectedItem = _quickSelections[2].GetComponentInChildren<UIItem>().item;
                UIManager.Instance.DisplayMessage("Player holds " + SelectedItem.title);
                AudioManager.Instance.PlayOneShot(FMODEvents.Instance.itemEquip, Vector3.zero);
            }
        }
    }
}
