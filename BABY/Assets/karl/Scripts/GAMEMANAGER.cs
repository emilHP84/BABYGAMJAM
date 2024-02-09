using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GAMEMANAGER : MonoBehaviour
{
    public static GAMEMANAGER access;
    [SerializeField] float dureePartie = 20f;
    float tempsActuel;
    bool playing= false;

    [RuntimeInitializeOnLoadMethod]
    static void Singleton()
    {
        if (SceneManager.GetActiveScene().buildIndex>0) SceneManager.LoadScene(0);
        Debug.Log("mon gros ROUDOUDOU");
        if (access!=null) return;
        GameObject gm = Instantiate(Resources.Load("GAME MANAGER", typeof(GameObject))) as GameObject;
        GameObject.DontDestroyOnLoad(gm);
        access = gm.GetComponent<GAMEMANAGER>();
        Debug.Log(access);
    }






    public void DebutPartie()
    {
        tempsActuel = 0;
        StartCoroutine(PartieEnCours());
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    Image clockFill;
    IEnumerator PartieEnCours()
    {
        playing = true;
        clockFill = GameObject.Find("Clockfill").GetComponent<Image>();
        while (tempsActuel< dureePartie)
        {
            tempsActuel += Time.deltaTime;
            clockFill.fillAmount = ((int)tempsActuel)/dureePartie;
            yield return null;
        }
        if (playing) Victoire();
    }

    public void Victoire()
    {
        if (playing==false) return;
        playing = false;
        Debug.Log("VICTORY");
    }

    public void GameOver()
    {
        if (playing==false) return;
        playing = false;
        Debug.Log("GAME OVER");
        GetComponent<MenuScript>().GameOver();
    }




} // FIN DU SCRIPT
