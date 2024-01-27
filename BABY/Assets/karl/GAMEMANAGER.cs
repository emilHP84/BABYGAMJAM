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

    void Awake()
    {
        if (access!=null) DestroyImmediate(this);
        access = this;
    }


    void Start()
    {
        tempsActuel = 0;
        StartCoroutine(PartieEnCours());
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
    }


} // FIN DU SCRIPT
