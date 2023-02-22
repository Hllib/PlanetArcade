using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Merchant : MonoBehaviour
{
    public bool TradeMode { get; set; }

    [SerializeField]
    private GameObject _shopPanel;

    [SerializeField]
    private Player3D _player;
    [SerializeField]
    private GameObject _visualCue;
    [SerializeField]
    private TextMeshProUGUI[] _itemsTitles;
    [SerializeField]
    private TextMeshProUGUI _goldAmount;

    private void Start()
    {
        TradeMode = false;
        _goldAmount.text = _player.goldAmount.ToString();
    }

    private void Update()
    {
        if (TradeMode && Input.GetKeyDown(KeyCode.E))
        {
            _visualCue.SetActive(false);
            _shopPanel.SetActive(true);
            _player.BlockMovement = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && TradeMode)
        {
            _visualCue.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && TradeMode)
        {
            _visualCue.SetActive(false);
        }
    }

    enum SelectedItem
    {
        Potion = 0,
        Shield = 1
    }

    enum SelectedCost
    {
        Potion = 2,
        Shield = 5,
    }

    public int currentSelectedItem;
    public int currentSelectedCost;

    public void SelectItem(int item)
    {
        switch (item)
        {
            case 0:
                _itemsTitles[0].color = Color.green;
                currentSelectedItem = (int)SelectedItem.Potion;
                currentSelectedCost = (int)SelectedCost.Potion;
                break;
            case 1:
                _itemsTitles[1].color = Color.green;
                currentSelectedItem = (int)SelectedItem.Shield;
                currentSelectedCost = (int)SelectedCost.Shield;
                break;
            default: break;
        }
    }

    public void BuyItem()
    {
        if (_player != null)
        {
            if (_player.goldAmount >= currentSelectedCost)
            {
                switch (currentSelectedItem)
                {
                    case (int)SelectedItem.Potion:
                        _player.GetComponent<Inventory>().GiveItem(InventoryTypes.Potion);
                        break;
                    case (int)SelectedItem.Shield:
                        _player.GetComponent<Inventory>().GiveItem(InventoryTypes.EnhancedShield);
                        break;
                }

                _player.goldAmount -= currentSelectedCost;
                _goldAmount.text = _player.goldAmount.ToString();

                for (int i = 0; i < currentSelectedCost; i++)
                {
                    _player.GetComponent<Inventory>().RemoveItem(InventoryTypes.Gold);
                }
            }
            else
            {
                return;
            }
        }
    }

    public void CloseShop()
    {
        _shopPanel.SetActive(false);
        _player.BlockMovement = false;
    }
}
