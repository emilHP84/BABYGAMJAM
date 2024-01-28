using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public enum BabyState{ idle, DoingTP, DoingLevitaion}
public class BabyFSM : MonoBehaviour
{
    public BabyState _babyState;
    [Header("Global Parameters")]
    public GameObject babyVisual;

    [Header("Teleportations Parameters")]
    public List<GameObject> babyTransformTP = new List<GameObject>();
    public float teleportationDuration = 3;

    [Header("Levitation Parameters")]
    public float levitationDuration = 3;

    [Header(" Idle Parameters")]
    public float idleDuration = 3;

    public ObjectPhysic[] objetProjete;
    float chrono;

    Vector3 newPos;
    

    void Start(){
        switchTo(BabyState.idle);
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
                if (chrono > teleportationDuration)
                {
                    babyVisual.SetActive(false);
                    if (chrono < levitationDuration + 0.1f) return;
                    gameObject.transform.position = newPos;
                    babyVisual.SetActive(true);
                    RandomizeState();
                }
            break;
        }
    }

    void switchTo(BabyState newState)
    {
        Debug.Log(newState);
        chrono = 0;
        _babyState = newState;
        switch (_babyState)
        {
            case BabyState.idle:

            break;

            case BabyState.DoingLevitaion:
                Debug.Log("coucou");
               objetProjete[Random.Range(0, objetProjete.Length)].StartProjection();
            break;

            case BabyState.DoingTP:
                Vector3 newPos = babyTransformTP[Random.Range(0, babyTransformTP.Count)].transform.position;
            break;
        }
    }

    void RandomizeState(){
        int i = Random.Range(0, 3);
        if (i == 0) switchTo(_babyState = BabyState.idle);
        else if (i == 1) switchTo(BabyState.DoingLevitaion);
        else if (i == 2) switchTo(_babyState = BabyState.DoingTP);
    }
}
