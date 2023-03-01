using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarsManager : MonoBehaviour
{
    [SerializeField]
    private Player _player;

    void Start()
    {
        StartCoroutine(SaveInv());
    }

    IEnumerator SaveInv()
    {
        yield return new WaitForSeconds(0.1f);

        //saving current inv and writing it to separate playerprefs string
        _player.SaveInventory();
        PlayerPrefs.SetString(PlayerSettings.WhenOnMarsInventory, PlayerPrefs.GetString(PlayerSettings.Inventory));
    }

    public void SetInventoryToMarsState()
    {
        PlayerPrefs.SetString(PlayerSettings.Inventory, PlayerPrefs.GetString(PlayerSettings.WhenOnMarsInventory));
    }
}
