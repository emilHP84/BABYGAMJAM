using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements.Experimental;
using System.Threading;
using System.Runtime.ExceptionServices;

public class MenuScript : MonoBehaviour
{
    [SerializeField] CanvasGroup blackScreen, startScreen, gameOverScreen;
    List<MenuEvent> stack;
    [SerializeField] AudioSource musique;

    [SerializeField]AudioClip menuMusic, gameOverSound;
    float musicTime;

    Camera menuCam;
    AudioListener listener;



    void Awake()
    {
        listener = GetComponentInChildren<AudioListener>();
        menuCam = GetComponentInChildren<Camera>();
        blackScreen.alpha = 1f;
        musique.volume = 0;
        menuMusic = musique.clip;
        stack = new List<MenuEvent>();
    }

    void Start()
    {
        stack.Add(new MenuDisplay(startScreen.gameObject));
        stack.Add(new MenuTransition(1f,0,2f,Ease.InOutSine));
        stack.Add(new MenuTransition(0,1f,2f,Ease.InOutSine));
        StartCoroutine(AnimationStackHandler());
    }



    IEnumerator AnimationStackHandler()
    {
        CursorLocker(true);
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
                RemoveAudioListeners();
                stack.Add(new MenuTransition(0,1f,2f,Ease.InOutSine));
            break;
            case AudioNarrative:
                AudioNarrative audioNarrative = (AudioNarrative)stack[0];
                musicTime = musique.time;
                musique.clip = audioNarrative.clip;
                musique.volume = 1f;
                musique.Stop();
                musique.Play();
                musique.loop = false;
                while (musique.isPlaying)
                {
                    if (Input.GetMouseButtonUp(0)) musique.Stop();
                    yield return null;
                }
            break;
            case WaitingTime:
                WaitingTime waitTime = (WaitingTime)stack[0];
                yield return new WaitForSecondsRealtime(waitTime.timer);
            break;
            case MusicResume:
                musique.clip = menuMusic;
                musique.time = musicTime;
                musique.Play();
            break;
            case CursorLock:
                CursorLock locker = (CursorLock)stack[0];
                CursorLocker(locker.wanted);
            break;
            case MenuDisplay:
                MenuDisplay menuDisplay = (MenuDisplay)stack[0];
                if (menuDisplay.menu==null)
                {
                    HideMenus();
                    menuCam.gameObject.SetActive(false);
                }

                else
                {
                    ShowMenu(menuDisplay.menu);
                    menuCam.gameObject.SetActive(true);
                }
            break;
        } // fin du switch


        if (blackScreen.alpha==0) blackScreen.gameObject.SetActive(false);
        stack.RemoveAt(0);
        CursorLocker(false);
        while (stack.Count<1) yield return null;
        StartCoroutine(AnimationStackHandler());
    }


    public void NewGame(AudioClip dialogueDebut)
    {
        Debug.Log("NEW GAME");
        stack.Add(new MenuTransition(1f,0,2f,Ease.InOutSine));
        stack.Add(new AudioNarrative(dialogueDebut));
        stack.Add(new WaitingTime(1f));
        stack.Add(new MenuDisplay());
        stack.Add(new SceneLoader(1));
    }


    public void GameOver()
    {
        stack.Add(new AudioNarrative(gameOverSound));
        stack.Add(new MenuTransition(1f,0,0.25f,Ease.InOutSine));
        stack.Add(new MenuDisplay(gameOverScreen.gameObject));
        stack.Add(new MenuTransition(0f,1,0.5f,Ease.InOutSine));
        stack.Add(new SceneLoader(0));
    }

    public void ReloadGame()
    {
        stack.Add(new MenuTransition(1f,0,0.25f,Ease.InOutSine));
        stack.Add(new MenuDisplay(startScreen.gameObject));
        musique.clip = menuMusic;
        musique.Play();
        stack.Add(new MenuTransition(0f,1,1.5f,Ease.InOutSine));
        stack.Add(new CursorLock(false));
    }

    void HideMenus()
    {
        startScreen.gameObject.SetActive(false);
        gameOverScreen.gameObject.SetActive(false);
    }

    void ShowMenu(GameObject desiredMenu)
    {
        HideMenus();
        desiredMenu.SetActive(true);
    }

    void CursorLocker(bool wanted)
    {
        Cursor.visible = !wanted;
        Cursor.lockState = CursorLockMode.None;
        if (wanted) Cursor.lockState = CursorLockMode.Locked;
    }

    void RemoveAudioListeners()
    {
        AudioListener[] listeners = GameObject.FindObjectsOfType<AudioListener>();
        for (int i=0;i<listeners.Length;i++)
        {
            if (listeners[i]!=listener) Destroy(listeners[i]);
        }
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
    public SceneLoader(int myScene)
    {
        desiredScene = myScene;
    }
    public int desiredScene;
}


public class AudioNarrative: MenuEvent
{
    public AudioNarrative(AudioClip myClip)
    {
        clip = myClip;
    }

    public AudioClip clip;
}

public class MusicResume: MenuEvent
{
    public MusicResume()
    {

    }
}


public class WaitingTime: MenuEvent
{
    public WaitingTime(float myTimer)
    {
        timer = myTimer;
    }
    public float timer;
}

public class CursorLock: MenuEvent
{
    public CursorLock(bool myWanted)
    {
        wanted = myWanted;
    }

    public bool wanted;
}

public class MenuDisplay: MenuEvent
{
    public MenuDisplay(GameObject myMenu)
    {
        menu = myMenu;
    }

    public MenuDisplay()
    {

    }
    public GameObject menu;
}