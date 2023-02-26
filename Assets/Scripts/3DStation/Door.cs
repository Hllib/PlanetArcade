using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : InteractableDoor
{
    public void OpenCloseSound()
    {
        AudioManager.Instance.PlayOneShot(FMODEvents.Instance.stationDoor, this.transform.position);
    }
}
