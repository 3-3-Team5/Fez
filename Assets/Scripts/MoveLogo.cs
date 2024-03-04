using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class MoveLogo : MonoBehaviour
{
    [SerializeField] private GameObject logo;
    [SerializeField] private GameObject Txt;
    private void Start()
    {
        logo.transform.DOLocalMove(new Vector3(0, 0), 2).SetEase(Ease.OutBounce).OnComplete(FadeIn);
    }

    private void FadeIn()
    {
        Txt.GetComponent<TextMeshProUGUI>().DOFade(1, 1f).OnComplete(Done);
    }

    private void Done() {
        
        gameObject.GetComponent<StartScene>().CanLoad = true;
    }
}