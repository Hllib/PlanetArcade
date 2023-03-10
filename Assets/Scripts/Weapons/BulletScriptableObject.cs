using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (fileName ="New Bullet", menuName ="ScriptableObjects/New Bullet")]
public class BulletScriptableObject : ScriptableObject
{
    public Sprite sprite;
    public int damage;
}
