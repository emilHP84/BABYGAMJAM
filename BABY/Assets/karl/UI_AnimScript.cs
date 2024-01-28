using UnityEngine;
using DG.Tweening;

public class UI_AnimScript : MonoBehaviour
{
    [SerializeField][Range(0,360f)] float rotateAngle;
    [SerializeField][Range(0.01f,10f)] float rotateTime = 1f;
    [SerializeField] Ease rotateEasing = Ease.InOutSine;

    [SerializeField][Range(0,360f)] float hoverOffset;
    [SerializeField][Range(0.01f,10f)] float hoverTime = 1f;
    [SerializeField] Ease hoverEasing = Ease.InOutSine;

    Vector3 startPos;

    void Awake()
    {
        startPos = transform.localPosition;
    }

    void OnEnable()
    {
        transform.DOKill();

        if (rotateAngle>0)
        {
            transform.localEulerAngles = Vector3.forward *-rotateAngle;
            transform.DOLocalRotate(Vector3.forward*2f*rotateAngle,rotateTime,RotateMode.LocalAxisAdd).SetLoops(-1,LoopType.Yoyo).SetEase(rotateEasing);
        }
        if (hoverOffset>0)
        {
            transform.localPosition = startPos;
            transform.DOLocalMoveY(startPos.y + hoverOffset,hoverTime).SetLoops(-1,LoopType.Yoyo).SetEase(hoverEasing);
        }
    }
} // FIN DU SCRIPT
