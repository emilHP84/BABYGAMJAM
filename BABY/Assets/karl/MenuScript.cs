using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MenuScript : MonoBehaviour
{
    [SerializeField] CanvasGroup blackScreen;
    List<MenuTransition> animStack;

    void Awake()
    {
        blackScreen.alpha = 1f;
        animStack = new List<MenuTransition>();
    }

    void Start()
    {
        animStack.Add(new MenuTransition(1f,2f));
        animStack.Add(new MenuTransition(0,2f));
        StartCoroutine(AnimationStackHandler());
    }



    IEnumerator AnimationStackHandler()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        blackScreen.gameObject.SetActive(true);
        blackScreen.DOKill();
        blackScreen.DOFade(animStack[0].desiredAlpha, animStack[0].duration);
        yield return new WaitForSecondsRealtime(animStack[0].duration);
        if (blackScreen.alpha==0) blackScreen.gameObject.SetActive(false);
        animStack.RemoveAt(0);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        while (animStack.Count<1) yield return null;
        StartCoroutine(AnimationStackHandler());
    }


} // FIN DU SCRIPT

public class MenuTransition
{
    public MenuTransition(float myDesiredAlpha, float myDuration)
    {
        duration = myDuration;
        desiredAlpha = myDesiredAlpha;
    }
    public float duration, desiredAlpha;
}
