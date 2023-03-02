using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Artifact : MonoBehaviour, IGatherable
{
    public Inventory PlayerInventory { get; set; }
    [SerializeField]
    private int _inventoryType;

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerInventory = other.GetComponent<Inventory>();
            PlayerInventory.GiveItem(_inventoryType);

            switch(_inventoryType)
            {
                case InventoryTypes.ArtifactMoon: 
                    UIManager.Instance.ShowArtifact(InventoryTypes.ArtifactMoon);
                    AudioManager.Instance.PlayOneShot(FMODEvents.Instance.winMusicCave, Vector3.zero);
                    break;
                case InventoryTypes.ArtifactMars:
                    UIManager.Instance.ShowArtifact(InventoryTypes.ArtifactMars);
                    break;
            }

            Destroy(gameObject);
        }
    }
}
