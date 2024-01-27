using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements.Experimental;

public class MenuScript : MonoBehaviour
{
    [SerializeField] CanvasGroup blackScreen;
    List<MenuEvent> stack;
    [SerializeField] AudioSource musique;

    void Awake()
    {
        //DontDestroyOnLoad(this.gameObject);
        blackScreen.alpha = 1f;
        musique.volume = 0;
        stack = new List<MenuEvent>();
    }

    void Start()
    {
        stack.Add(new MenuTransition(1f,0,2f,Ease.InOutSine));
        stack.Add(new MenuTransition(0,1f,2f,Ease.InOutSine));
        StartCoroutine(AnimationStackHandler());
    }



    IEnumerator AnimationStackHandler()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        blackScreen.gameObject.SetActive(true);
        blackScreen.DOKill();
        musique.DOKill();

        switch (stack[0])
        {
            case MenuTransition:
                MenuTransition menuTransition = (MenuTransition)stack[0];
                musique.DOFade(menuTransition.musicVolume, menuTransition.duration);
                blackScreen.DOFade(menuTransition.desiredAlpha, menuTransition.duration).SetEase(menuTransition.easing);
                yield return new WaitForSecondsRealtime(menuTransition.duration);
            break;
            case SceneLoader:
                SceneLoader sceneLoader = (SceneLoader)stack[0];
                AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneLoader.desiredScene,LoadSceneMode.Single);
                while (!asyncLoad.isDone)
                yield return null;
            break;
        }


        if (blackScreen.alpha==0) blackScreen.gameObject.SetActive(false);
        stack.RemoveAt(0);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        while (stack.Count<1) yield return null;
        StartCoroutine(AnimationStackHandler());
    }

    public void LaunchScene(string desiredScene)
    {
        stack.Add(new MenuTransition(1f,0,2f,Ease.InOutSine));
        stack.Add(new SceneLoader(desiredScene));
    }


} // FIN DU SCRIPT

public class MenuEvent
{

}


public class MenuTransition: MenuEvent
{
    public MenuTransition(float myDesiredAlpha, float myMusicVolume, float myDuration, Ease myEasing)
    {
        duration = myDuration;
        desiredAlpha = myDesiredAlpha;
        easing = myEasing;
        musicVolume = myMusicVolume;
    }
    public float duration, desiredAlpha, musicVolume;
    public Ease easing;
}


public class SceneLoader: MenuEvent
{
    public SceneLoader(string myScene)
    {
        desiredScene = myScene;
    }
    public string desiredScene;
}
