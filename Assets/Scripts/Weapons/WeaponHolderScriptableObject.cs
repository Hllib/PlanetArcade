using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "ScriptableObjects/WeaponHolder")]
public class WeaponHolderScriptableObject : ScriptableObject
{
    public List<WeaponScriptableObject> weapons;
}
