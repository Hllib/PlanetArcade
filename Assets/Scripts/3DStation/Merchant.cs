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
    private bool _playerInRange;

    private void Start()
    {
        TradeMode = false;
    }

    private void Update()
    {
        if (TradeMode && Input.GetKeyDown(KeyCode.E) && _playerInRange)
        {
            _goldAmount.text = _player.goldAmount.ToString();
            _visualCue.SetActive(false);
            _shopPanel.SetActive(true);
            _player.BlockMovement = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && TradeMode)
        {
            _playerInRange = true;
            _visualCue.SetActive(true);
            AudioManager.Instance.PlayOneShot(FMODEvents.Instance.eButton, Vector3.zero);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && TradeMode)
        {
            _playerInRange = false;
            _visualCue.SetActive(false);
        }
    }

    enum SelectedItem
    {
        Potion = 0,
        Shield = 1,
        Rifle = 2
    }

    enum SelectedCost
    {
        Potion = 2,
        Shield = 6,
        Rifle = 3
    }

    public int currentSelectedItem;
    public int currentSelectedCost;

    public void SelectItem(int item)
    {
        switch (item)
        {
            case 0:
                _itemsTitles[0].color = Color.green;
                _itemsTitles[1].color = Color.white;
                _itemsTitles[2].color = Color.white;
                currentSelectedItem = (int)SelectedItem.Potion;
                currentSelectedCost = (int)SelectedCost.Potion;
                break;
            case 1:
                _itemsTitles[0].color = Color.white;
                _itemsTitles[1].color = Color.green;
                _itemsTitles[2].color = Color.white;
                currentSelectedItem = (int)SelectedItem.Shield;
                currentSelectedCost = (int)SelectedCost.Shield;
                break;
            case 2:
                _itemsTitles[0].color = Color.white;
                _itemsTitles[1].color = Color.white;
                _itemsTitles[2].color = Color.green;
                currentSelectedItem = (int)SelectedItem.Rifle;
                currentSelectedCost = (int)SelectedCost.Rifle;
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
                _player.goldAmount -= currentSelectedCost;
                _goldAmount.text = _player.goldAmount.ToString();

                Inventory playerInv = _player.GetComponent<Inventory>();

                for (int i = 0; i < currentSelectedCost; i++)
                {
                    playerInv.RemoveItem(InventoryTypes.Gold);
                }

                switch (currentSelectedItem)
                {
                    case (int)SelectedItem.Potion:
                        playerInv.GiveItem(InventoryTypes.Potion);
                        break;
                    case (int)SelectedItem.Shield:
                        playerInv.GiveItem(InventoryTypes.EnhancedShield);
                        break;
                    case (int)SelectedItem.Rifle:
                        playerInv.GiveItem(InventoryTypes.Rifle);
                        break;
                }

                AudioManager.Instance.PlayOneShot(FMODEvents.Instance.buyItem, Vector3.zero);
            }
            else
            {
                AudioManager.Instance.PlayOneShot(FMODEvents.Instance.notEnoughGold, Vector3.zero);

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
