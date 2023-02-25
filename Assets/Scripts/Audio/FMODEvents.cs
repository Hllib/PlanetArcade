using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class FMODEvents : MonoBehaviour
{
    [field: Header("Player SFX")]
    [field: SerializeField]
    public EventReference playerFootstepsWalk { get; private set; }
    [field: SerializeField]
    public EventReference playerFootstepsSprint { get; private set; }
    [field: SerializeField]
    public EventReference goldCollected { get; private set; }
    [field: SerializeField]
    public EventReference ambience { get; private set; }
    [field: SerializeField]
    public EventReference music { get; private set; }
    [field: SerializeField]
    public EventReference buttonEnter { get; private set; }
    [field: SerializeField]
    public EventReference buttonClick { get; private set; }
    [field: SerializeField]
    public EventReference invSlot_hover { get; private set; }
    [field: SerializeField]
    public EventReference invSlot_select { get; private set; }
    [field: SerializeField]
    public EventReference chestOpen { get; private set; }

    private static FMODEvents _instance;

    public static FMODEvents Instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.LogError("FMODEvents is NULL :: FMODEvents.cs");
            }

            return _instance;
        }
    }

    private void Awake()
    {
        _instance = this;
    }


}
