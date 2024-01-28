using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

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


    IEnumerator PartieEnCours()
    {
        playing = true;
        while (tempsActuel< dureePartie)
        {
            tempsActuel += Time.deltaTime;
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
