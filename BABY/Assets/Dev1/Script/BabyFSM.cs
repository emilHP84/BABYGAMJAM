using System.Collections.Generic;
using UnityEngine;

public enum BabyState{Idle, PreparingTP, DoingTP, LevitatingObject}
public class BabyFSM : MonoBehaviour
{
    public BabyState currentState;

    [Header("Global Parameters")]
    [SerializeField] Transform babyPoses;
    [SerializeField] ParticleSystem particuleTP;

    [Header("Teleportations Parameters")]
    [SerializeField] Transform teleportLocations;
    [SerializeField] float teleportationDuration = 8f;


    [Header(" Idle Parameters")]
    [SerializeField] float idleDuration = 5f;

    public ObjectPhysic[] objetProjete;
    float chrono;

    Vector3 newPos;
    

    void Start()
    {
        SwitchTo(BabyState.Idle);
        ShowRandomPose();
        newPos = teleportLocations.GetChild(0).position;
        GAMEMANAGER.access.DebutPartie();
    }

    

    void Update()
    {
        chrono += Time.deltaTime;

        switch (currentState) // UPDATE STATE
        {
            case BabyState.Idle:
                if (chrono > idleDuration)
                {
                    float randomizer = Random.value;
                    if (randomizer>0.7f) SwitchTo(BabyState.LevitatingObject);
                    else SwitchTo(BabyState.PreparingTP);
                }
            break;

            case BabyState.LevitatingObject:
            break;

            case BabyState.PreparingTP:
                if (particuleTP != null) particuleTP.Play();
                if (chrono > teleportationDuration)
                {
                    SwitchTo(BabyState.Idle);
                }
            break;

            case BabyState.DoingTP:
            
            break;
        } // fin du switch
    } // fin de Update()


    void SwitchTo(BabyState newState)
    {
        switch (currentState) // EXIT STATE FUNCTION
        {
            case BabyState.PreparingTP: if (particuleTP != null) particuleTP.Stop(); break;
        }

        chrono = 0;
        currentState = newState;

        switch (newState) // ENTER STATE FUNCTION
        {
            case BabyState.Idle:
            break;

            case BabyState.LevitatingObject:
                objetProjete[Random.Range(0, objetProjete.Length)].StartProjection();
            break;

            case BabyState.PreparingTP:
                if (particuleTP != null) particuleTP.Play();
            break;

            case BabyState.DoingTP:
                Vector3 chosingNewPosition = teleportLocations.GetChild(Random.Range(0,teleportLocations.childCount)).position;
                while (chosingNewPosition==newPos)
                    chosingNewPosition = teleportLocations.GetChild(Random.Range(0,teleportLocations.childCount)).position;
                newPos = chosingNewPosition;
                
                babyPoses.gameObject.SetActive(false);
                //if (chrono < teleportationDuration + 0.1f) return;
                gameObject.transform.position = newPos;
                ShowRandomPose();
                babyPoses.gameObject.SetActive(true);
                SwitchTo(BabyState.Idle);
            break;
        }

    }



    void ShowRandomPose()
    {
        for (int i  = 0; i < babyPoses.childCount; i ++)
            babyPoses.GetChild(i).gameObject.SetActive(false);
        babyPoses.GetChild(Random.Range(0, babyPoses.childCount)).gameObject.SetActive(true);
    }

    public void EndObjectLevitation()
    {
        SwitchTo(BabyState.Idle);
    }

} // FIN DU SCRIPT
