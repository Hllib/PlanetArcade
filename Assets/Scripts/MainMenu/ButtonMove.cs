using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonMove : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    private VerticalLayoutGroup _layoutGroup;
    private Animator _buttonAnimator;

    void Start()
    {
        _layoutGroup = GetComponentInParent<VerticalLayoutGroup>();
        _buttonAnimator = GetComponent<Animator>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _layoutGroup.enabled = false;
        _buttonAnimator.SetTrigger("Enter");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
         _layoutGroup.enabled = true;
        _buttonAnimator.SetTrigger("Exit");
    }

}
