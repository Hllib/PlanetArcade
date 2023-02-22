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
    [SerializeField] 
    private AudioClip _buttonEnter;
    [SerializeField]
    private AudioClip _buttonPressed;

    void Start()
    {
        _layoutGroup = GetComponentInParent<VerticalLayoutGroup>();
        _buttonAnimator = GetComponent<Animator>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _layoutGroup.enabled = false;
        _buttonAnimator.SetTrigger("Enter");
        AudioManager.Instance.PlayClip(_buttonEnter);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
         _layoutGroup.enabled = true;
        _buttonAnimator.SetTrigger("Exit");
    }

    public void PressedSound()
    {
        AudioManager.Instance.PlayClip(_buttonPressed);
    }
}
