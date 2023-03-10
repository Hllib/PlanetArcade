using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    public GameObject poolObject;
    public int poolAmount;
    public bool willGrow;

    private List<GameObject> poolList;

    private static ObjectPooler _instance;

    public static ObjectPooler Instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.LogError("ObjectPooler is NULL");
            }

            return _instance;
        }
    }

    private void Awake()
    {
        _instance = this;
    }

    private void Start()
    {
        poolList = new List<GameObject>();
        for (int i = 0; i < poolAmount; i++)
        {
            GameObject obj = Instantiate(poolObject);
            obj.transform.SetParent(transform, true);
            obj.SetActive(false);
            poolList.Add(obj);
        }
    }

    public GameObject GetPooledObject()
    {
        for(int i = 0; i < poolList.Count; i++)
        {
            if (!poolList[i].activeInHierarchy)
            {
                return poolList[i];
            }

            if(willGrow)
            {
                GameObject obj = Instantiate(poolObject);
                poolList.Add(obj);
                return obj;
            }
        }

        return null;
    }
}
