using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class WeaponGraphicController : MonoBehaviour
{
    [SerializeField]
    private GameObject _coverPrefab;
    private SpriteRenderer _coverPrefabRenderer;
    [SerializeField]
    public GameObject[] _weaponPosPrefabs;
    [SerializeField]
    private int _inventoryId;

    public int InventoryId { get { return _inventoryId; } private set { _inventoryId = value; } }

    private void Start()
    {
        _coverPrefabRenderer = _coverPrefab.GetComponent<SpriteRenderer>();
    }

    enum LookDirection
    {
        Left,
        Right,
        Front,
        Back,
        LB,
        RB,
        RF,
        LF
    }

    public void SwitchWeaponPosition(int weaponDirection)
    {
        _weaponPosPrefabs[weaponDirection].SetActive(true);
        if (weaponDirection == (int)LookDirection.Left || weaponDirection == (int)LookDirection.LB || weaponDirection == (int)LookDirection.LF)
        {
            _coverPrefabRenderer.flipX = false;
            _coverPrefab.SetActive(true);
        }
        if (weaponDirection == (int)LookDirection.Right || weaponDirection == (int)LookDirection.RB || weaponDirection == (int)LookDirection.RF)
        {
            _coverPrefabRenderer.flipX = true;
            _coverPrefab.SetActive(true);
        }
    }

    public void HideGraphics()
    {
        _weaponPosPrefabs.Single(prefab => prefab.activeSelf).SetActive(false);
        _coverPrefab.SetActive(false);
    }
}
