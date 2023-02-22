using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldItem : MonoBehaviour, IGatherable
{
    [SerializeField]
    private bool _isTutorial;
    [SerializeField]
    private GameObject _shieldTutorial;
    private bool _hasAdded;

    public Inventory PlayerInventory { get; set; }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (!_hasAdded)
            {
                PlayerInventory = other.GetComponent<Inventory>();
                PlayerInventory.GiveItem(InventoryTypes.Shield);
                other.GetComponent<Player>().HasShield(true);
                _hasAdded = true;
            }

            if (_isTutorial)
            {
                Guide.Instance.PickUpShield();
                
            }
            Destroy(gameObject);
        }
    }
}
