using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class tom_script : MonoBehaviour
{
    [SerializeField][Range(0.01f,360f)] float scaleanim;
    [SerializeField][Range(0.01f,10f)] float scaleTime = 1f;
    [SerializeField] Ease scaleEasing = Ease.InOutSine;
    void Start()
    {
        transform.DOScaleY(scaleanim, scaleTime).SetLoops(-1, LoopType.Yoyo).SetEase(scaleEasing);
    }

}
