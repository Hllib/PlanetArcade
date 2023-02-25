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
    public EventReference itemInteraction { get; private set; }
    [field: SerializeField]
    public EventReference chestOpen { get; private set; }
    [field: SerializeField]
    public EventReference itemEquip { get; private set; }
    [field: SerializeField]
    public EventReference pistolFire { get; private set; }
    [field: SerializeField]
    public EventReference rifleFire { get; private set; }
    [field: SerializeField]
    public EventReference slimeJump { get; private set; }
    [field: SerializeField]
    public EventReference slimeDeath { get; private set; }
    [field: SerializeField]
    public EventReference slimeAttack { get; private set; }
    [field: SerializeField]
    public EventReference hit { get; private set; }
    [field: SerializeField]
    public EventReference playerDeath { get; private set; }
    [field: SerializeField]
    public EventReference playerHit { get; private set; }

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
