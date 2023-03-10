using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSettings : MonoBehaviour
{
    //These are the names of the playerPrefs 
    public const string Earth = "Earth";
    public const string Moon = "Moon";
    public const string Mars = "Mars";
    public const string Station = "Station3D";

    public const string MasterVolume = "MasterVolume";
    public const string MusicVolume = "MusicVolume";
    public const string AmbienceVolume = "AmbienceVolume";
    public const string SFXVolume = "SFXVolume";

    public const string Inventory = "Inventory";
    public const string WhenOnMarsInventory = "OnMarsInventory";

    public const int Done = 1;
    public const int NotDone = 0;

    public const int Health = 4;

    public const string GameStarted = "GameStarted";
    public const string GameFinished = "GameFinished";


    public const string GotAchievement = "ShowAchievement";
}
