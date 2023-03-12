using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemFollowMouse : MonoBehaviour
{
    [SerializeField]
    private bool _worldPosition;

    private void LateUpdate()
    {
        transform.position = Input.mousePosition;
        if(_worldPosition)
        {
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = Camera.main.nearClipPlane;
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePos);
            transform.position = worldPosition;
        }
    }
}
