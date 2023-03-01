using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortToBoss : MonoBehaviour
{
    [SerializeField]
    private GameObject _fade;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            StartCoroutine(MoveToBoss());
        }
    }

    IEnumerator MoveToBoss()
    {
        _fade.SetActive(true);
        yield return new WaitForSeconds(1);
        GameManager.Instance.MoveToFinalBoss();
    }
}
