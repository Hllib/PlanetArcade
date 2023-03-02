using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortToScene : MonoBehaviour
{
    [SerializeField]
    private GameObject _fade;

    [SerializeField]
    private string _sceneName;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            switch(_sceneName)
            {
                case "BossFight": StartCoroutine(MoveToBoss()); break;   
                case "Final": StartCoroutine(MoveToBoss()); break;   
            }
        }
    }

    IEnumerator MoveToBoss()
    {
        _fade.SetActive(true);
        yield return new WaitForSeconds(1);
        GameManager.Instance.MoveToFinalBoss();
    }

    IEnumerator MoveToFinal()
    {
        _fade.SetActive(true);
        yield return new WaitForSeconds(1);
    }
}
