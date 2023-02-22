using System.Collections;
using System.Collections.Generic;
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
}
