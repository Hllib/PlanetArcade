using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Gateway : InteractableDoor
{
    [SerializeField]
    private GameObject _leavePanel;
    [SerializeField]
    private GameObject _fade;

    public override void Update()
    {
        if (player != null)
        {
            if (player.HasInteracted && playerInRange)
            {
                player.HasInteracted = false;

                _leavePanel.SetActive(true);
                visualCue.SetActive(false);
            }
        }
    }

    public void CancelDeparture()
    {
        _leavePanel.SetActive(false);
    }

    public void Depart()
    {
        StringBuilder stringOfId = new StringBuilder();

        foreach (var item in player.playerInventory.playerItems)
        {
            stringOfId.Append(item.id);
            stringOfId.Append(" ");
        }

        PlayerPrefs.SetString(PlayerSettings.Inventory, stringOfId.ToString());
        PlayerPrefs.Save();

        animator.SetBool("Open", !animator.GetBool("Open"));
        player.BlockMovement = true;
        StartCoroutine(LeaveTheStation());
        _leavePanel.SetActive(false);
    }

    IEnumerator LeaveTheStation()
    {
        yield return new WaitForSeconds(2f);
        _fade.SetActive(true);
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene("PlanetsMenu");
        
        player.BlockMovement = false;
    }

    public void OpenCloseSound()
    {
        AudioManager.Instance.PlayOneShot(FMODEvents.Instance.stationGateway, this.transform.position);
    }
}
