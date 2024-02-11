using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Unity.VisualScripting;

public enum BabyState{Idle, PreparingTP, DoingTP, LevitatingObject, ReturningToCraddle}
public class BabyFSM : MonoBehaviour, IInteractable
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
    [SerializeField]Transform objetsAProjeter;
    List<ObjectPhysic> objetsDispo;
    ObjectPhysic[] objetProjete;
    float chrono;
    float chronoBlink;

    Vector3 newPos;
    [SerializeField] AudioClip teleportSound;
    [SerializeField] AudioClip[] babySounds;
    [SerializeField] AudioSource tpChargeSound;
    [SerializeField] GameObject spoofParticles;
    Vector3 startPos;

    [SerializeField]GameObject colliders;


    
    void Awake()
    {
        startPos = transform.position;
        newPos = teleportLocations.GetChild(0).position;
        objetProjete = objetsAProjeter.GetComponentsInChildren<ObjectPhysic>();
        objetsDispo = new List<ObjectPhysic>();
        foreach (ObjectPhysic objet in objetProjete) objetsDispo.Add(objet);
    }

    void Start()
    {
        SwitchTo(BabyState.Idle);
        ShowRandomPose();
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
                    if (randomizer>0.5f) SwitchTo(BabyState.LevitatingObject);
                    else SwitchTo(BabyState.PreparingTP);
                }
            break;

            case BabyState.LevitatingObject:
            break;

            case BabyState.PreparingTP:
                if (particuleTP != null) particuleTP.Play();
                if (chronoBlink<chrono)
                {
                    babyPoses.gameObject.SetActive(!babyPoses.gameObject.activeSelf);
                    chronoBlink += 0.1f;
                }
                if (chrono > teleportationDuration)
                    SwitchTo(BabyState.DoingTP);      
            break;

            case BabyState.DoingTP:
            
            break;
        } // fin du switch
    } // fin de Update()






    void SwitchTo(BabyState newState)
    {
        Debug.Log("Switch from "+currentState+" to "+newState);

        switch (currentState) // EXIT STATE FUNCTION
        {
            case BabyState.PreparingTP:
                if (particuleTP != null) particuleTP.Stop();
                tpChargeSound.Pause();
                babyPoses.gameObject.SetActive(true);
            break;
            case BabyState.ReturningToCraddle:
                colliders.SetActive(true);
            break;
        }

        chrono = 0;
        currentState = newState;

        switch (newState) // ENTER STATE FUNCTION
        {
            case BabyState.Idle:
                Sound.access.PlayWithDelay(babySounds[Random.Range(0,babySounds.Length)], .3f,Random.Range(0.5f,2f));
            break;

            case BabyState.LevitatingObject:
                objetsDispo = new List<ObjectPhysic>();
                for (int i=0; i<objetProjete.Length;i++)
                    if (objetProjete[i].currentState!=ObjectPhysic.State.Start) objetsDispo.Add(objetProjete[i]);
                objetsDispo[Random.Range(0, objetsDispo.Count)].StartProjection();
            break;

            case BabyState.PreparingTP:
                if (particuleTP != null) particuleTP.Play();
                tpChargeSound.Play();
                chronoBlink = 1f;
            break;

            case BabyState.DoingTP:
                Vector3 chosingNewPosition = teleportLocations.GetChild(Random.Range(0,teleportLocations.childCount)).position;
                while (chosingNewPosition==newPos)
                    chosingNewPosition = teleportLocations.GetChild(Random.Range(0,teleportLocations.childCount)).position;
                newPos = chosingNewPosition;
                babyPoses.gameObject.SetActive(false);
                //if (chrono < teleportationDuration + 0.1f) return;
                Sound.access.Play(teleportSound, 1f);
                Instantiate(spoofParticles,transform.position,transform.rotation);
                transform.position = newPos;
                ShowRandomPose();
                babyPoses.gameObject.SetActive(true);
                Instantiate(spoofParticles,transform.position,transform.rotation);
                SwitchTo(BabyState.Idle);
            break;

            case BabyState.ReturningToCraddle:
            colliders.SetActive(false);
            transform.DOKill();
            transform.DOLocalRotate(Vector3.up*360f,0.5f,RotateMode.LocalAxisAdd);
            transform.DOJump(startPos,2f,1,0.5f).OnComplete(EndObjectLevitation);
            //transform.DOMove(startPos,0.5f).SetEase(Ease.InOutSine).OnComplete(EndObjectLevitation);
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

    public void MouseHover()
    {
        hovered=true;
    }

    public void MouseUnhover()
    {
        hovered=false;
    }

    public void OnDisable()
    {
        hovered=false;
    }

    [SerializeField] Transform babyPivot;

    public void MouseClicDown()
    {
        babyPivot.localScale = Vector3.one;
        babyPivot.DOKill();
        babyPivot.DOPunchScale(Vector3.one*0.1f,1f,4,0);

        Sound.access.Play(babySounds[Random.Range(0,babySounds.Length)],1f);
        if (currentState==BabyState.PreparingTP)
        {
            SwitchTo(BabyState.Idle);
        }
        else if (transform.position!=startPos) SwitchTo(BabyState.ReturningToCraddle);
    }


    bool hovered;
    public bool Hovered
    {
        get {return hovered;}
    }

} // FIN DU SCRIPT
