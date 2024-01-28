using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public enum BabyState{ idle, DoingTP, DoingLevitaion}
public class BabyFSM : MonoBehaviour
{
    public BabyState _babyState;

    [Header("Global Parameters")]
    public List<GameObject> babyPoses = new List<GameObject>();
    public GameObject babyVisual;
    public ParticleSystem particuleTP;

    [Header("Teleportations Parameters")]
    public Transform[] babyTransformTP;
    public float teleportationDuration = 8f;

    [Header("Levitation Parameters")]
    public float levitationDuration = 8f;

    [Header(" Idle Parameters")]
    public float idleDuration = 5f;

    public ObjectPhysic[] objetProjete;
    float chrono;
    bool asAlreadyTP;

    Vector3 newPos;
    

    void Start()
    {
        switchTo(BabyState.idle);
        GAMEMANAGER.access.DebutPartie();
        RandomizeMesh();
        asAlreadyTP = false;
        newPos = babyTransformTP[0].position;
    }

    

    void Update()
    {
        chrono += Time.deltaTime;

        switch (_babyState)
        {
            case BabyState.idle:
                if (chrono > idleDuration) RandomizeState();
            break;

            case BabyState.DoingLevitaion:
                if (chrono > levitationDuration) RandomizeState();
            break;

            case BabyState.DoingTP:
                if(particuleTP != null)particuleTP.Play();
                if (chrono > teleportationDuration)
                {
                    babyVisual.SetActive(false);
                    if (chrono < teleportationDuration + 0.1f) return;
                    gameObject.transform.position = newPos;
                    RandomizeMesh();
                    babyVisual.SetActive(true);
                    RandomizeState();
                    if (particuleTP != null) particuleTP.Stop();
                }
            break;
        }
    }

    void switchTo(BabyState newState)
    {
        //Debug.Log(newState);
        chrono = 0;
        _babyState = newState;
        switch (_babyState)
        {
            case BabyState.idle:
                asAlreadyTP = false;
                break;

            case BabyState.DoingLevitaion:
                //Debug.Log("coucou");
                asAlreadyTP = false;
                objetProjete[Random.Range(0, objetProjete.Length)].StartProjection();
            break;

            case BabyState.DoingTP:
                asAlreadyTP = true;
                Vector3 chosingNewPosition = babyTransformTP[Random.Range(0, babyTransformTP.Length)].position;
                while (chosingNewPosition==newPos)
                {
                    chosingNewPosition = babyTransformTP[Random.Range(0, babyTransformTP.Length)].position;
                }
                newPos = chosingNewPosition;

                newPos = babyTransformTP[Random.Range(0, babyTransformTP.Length)].position;
            break;
        }
    }

    void RandomizeState(){
        int i = Random.Range(0, 3);
        if (i == 0) switchTo(_babyState = BabyState.idle);
        else if (i == 1) switchTo(BabyState.DoingLevitaion);
        else if (i == 2) if (asAlreadyTP) RandomizeState(); else switchTo(_babyState = BabyState.DoingTP);
    }

    void RandomizeMesh(){
        for (int i  = 0; i < babyPoses.Count; i ++){
            babyPoses[i].gameObject.SetActive(false);
        }
        int j = Random.Range(0, babyPoses.Count);
        babyPoses[j].gameObject.SetActive(true);
    }
}
