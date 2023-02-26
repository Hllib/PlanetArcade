using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InteractableDoor : MonoBehaviour
{
    [SerializeField]
    protected Animator animator;
    [SerializeField]
    protected GameObject visualCue;
    [SerializeField]
    protected Player3D player;

    protected float canInteractTime = 0.0f;
    protected float interactRate = 1.5f;

    protected bool playerInRange;

    public virtual void Update()
    {
        if (player != null)
        {
            if (player.HasInteracted && Time.time > canInteractTime && playerInRange)
            {
                player.HasInteracted = false;
                canInteractTime = Time.time + interactRate;

                animator.SetBool("Open", !animator.GetBool("Open"));
                visualCue.SetActive(false);
            }
            else if (player.HasInteracted && Time.time < canInteractTime && playerInRange)
            {
                player.HasInteracted = false;
            }
        }
    }

    public virtual void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            if (player == null)
            {
                player = other.GetComponent<Player3D>();
            }
            if (Time.time > canInteractTime)
            {
                AudioManager.Instance.PlayOneShot(FMODEvents.Instance.eButton, Vector3.zero);
                visualCue.SetActive(true);
            }
        }
    }

    public virtual void OnTriggerExit(Collider other)
    {
        playerInRange = false;
        if (other.CompareTag("Player"))
        {
            visualCue.SetActive(false);
        }
    }
}
