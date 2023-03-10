using FMODUnity;
using System;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "ScriptableObjects/New Weapon")]
public class WeaponScriptableObject : ScriptableObject
{
    public string title;
    public Sprite sprite;
    public float scaleFactor;
    public float fireForce;

    public EventReference shootSound;
    
    [Serializable]
    public struct ShootStartPoints
    {
        public float X;
        public float Y;
    }

    public ShootStartPoints shootStartPoints;
    public BulletScriptableObject bulletScriptableObject;
}
