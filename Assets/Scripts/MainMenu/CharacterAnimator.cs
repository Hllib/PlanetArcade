using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterAnimator : MonoBehaviour
{
    private Image _characterImg;
    private Animator _characterAnimator;
    private int _animCount = 0;

    private void Start()
    {
        _characterImg = GetComponent<Image>();
        _characterAnimator = GetComponent<Animator>();
        _animCount = _characterAnimator.runtimeAnimatorController.animationClips.Length; // number of nonempty states 
        _characterImg.rectTransform.localScale = new Vector3(-4, 4, 0);

        StartCoroutine(Test());
    }

    IEnumerator Test()
    {
        while (true)
        {
            yield return new WaitForSecondsRealtime(_characterAnimator.GetCurrentAnimatorStateInfo(0).length);
            yield return new WaitForSecondsRealtime(_characterAnimator.GetCurrentAnimatorStateInfo(0).length);
            ChooseRandomAnimation();
        }
    }

    void ChooseRandomAnimation()
    {
        int randomAnimId = Random.Range(1, _animCount + 1);

        switch (randomAnimId)
        {
            case 1: Trigger("Idle"); break;
            case 2: Trigger("LookSide"); break;
            case 3: Trigger("SitFront"); break;
            case 4: Trigger("SitSide"); break;
        }

    }

    private void Trigger(string triggerName)
    {
        _characterAnimator.SetTrigger(triggerName);
    }
}
