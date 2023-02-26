using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class FMODEvents : MonoBehaviour
{
    [field: Header("PLAYER")]
    [field: SerializeField]
    public EventReference playerFootstepsWalk { get; private set; }
    [field: SerializeField]
    public EventReference playerFootstepsSprint { get; private set; }
    [field: SerializeField]
    public EventReference goldCollected { get; private set; }
    [field: SerializeField]
    public EventReference pistolFire { get; private set; }
    [field: SerializeField]
    public EventReference rifleFire { get; private set; }
    [field: SerializeField]
    public EventReference playerDeath { get; private set; }
    [field: SerializeField]
    public EventReference shieldDeflect { get; private set; }

    [field: Header("ENEMIES")]
    [field: SerializeField]
    public EventReference slimeJump { get; private set; }
    [field: SerializeField]
    public EventReference slimeDeath { get; private set; }
    [field: SerializeField]
    public EventReference slimeAttack { get; private set; }
    [field: SerializeField]
    public EventReference hit { get; private set; }
    [field: SerializeField]
    public EventReference fire { get; private set; }
    [field: SerializeField]
    public EventReference fireBig { get; private set; }
    [field: SerializeField]
    public EventReference enemyFootstep { get; private set; }
    [field: SerializeField]
    public EventReference enemyDeath { get; private set; }

    [field: Header("OBJECTS")]
    [field: SerializeField]
    public EventReference chestOpen { get; private set; }
    
    [field: Header("UI")]
    [field: SerializeField]
    public EventReference buttonEnter { get; private set; }
    [field: SerializeField]
    public EventReference buttonClick { get; private set; }
    [field: SerializeField]
    public EventReference itemInteraction { get; private set; }
    [field: SerializeField]
    public EventReference itemEquip { get; private set; }
    [field: SerializeField]
    public EventReference buyItem { get; private set; }
    [field: SerializeField]
    public EventReference notEnoughGold { get; private set; }
    [field: SerializeField]
    public EventReference rocketFly { get; private set; }

    [field: Header("GENERAL")]
    [field: SerializeField]
    public EventReference ambience { get; private set; }
    [field: SerializeField]
    public EventReference music { get; private set; }
    [field: SerializeField]
    public EventReference winMusicCave { get; private set; }

    [field: Header("STATION")]
    [field: SerializeField]
    public EventReference stationDoor { get; private set; }
    [field: SerializeField]
    public EventReference stationGateway { get; private set; }
    [field: SerializeField]
    public EventReference eButton { get; private set; }
    [field: SerializeField]
    public EventReference footsteps { get; private set; }

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
