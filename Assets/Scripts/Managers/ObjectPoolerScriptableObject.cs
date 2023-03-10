using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ObjectPooler", menuName = "ScriptableObjects/New Object Pooler")]
public class ObjectPoolerScriptableObject : ScriptableObject
{
    public GameObject poolObject;
    public int poolAmount;
    public bool willGrow;
}
