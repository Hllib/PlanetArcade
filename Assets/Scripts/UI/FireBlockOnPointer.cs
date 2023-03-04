using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class FireBlockOnPointer : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    private Player _player;

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (_player != null)
        {
            _player.FireBlocked = true;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (_player != null)
        {
            _player.FireBlocked = false;
;        }
    }
}
