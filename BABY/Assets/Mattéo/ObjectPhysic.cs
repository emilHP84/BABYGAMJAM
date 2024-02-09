using DG.Tweening;
using UnityEngine;

public class ObjectPhysic : MonoBehaviour, IInteractable
{
    [Range(1f,1000f)][SerializeField] float breakVelocity = 1000f;
    public enum State{Start,Ragdoll,Levitating,Ballistic,Resetting,Destroyed}
    public State currentState;
    new Rigidbody rigidbody;
    [Range(0f, 100f)] public float forcePower = 50;
    Vector3 force;

    public AudioSource woosh;
    static BabyFSM baby;
    Vector3 startPos, startRot,startScale;
    [SerializeField]GameObject spoofParticles;
    float chrono;
    TrailRenderer trail;
    ToGlow glow;
    [SerializeField] ParticleSystem levitatingParticles;
    [SerializeField] GameObject breakParticles;

    void Awake()
    {
        glow = GetComponent<ToGlow>();
        trail = GetComponentInChildren<TrailRenderer>();
        startPos = transform.position;
        startRot = transform.localEulerAngles;
        startScale = transform.localScale;
        baby = GameObject.FindObjectOfType<BabyFSM>();
        rigidbody = GetComponent<Rigidbody>();                                                          
    }

    void Start()
    {
        EnterState(State.Start);
    }

    public void StartProjection()
    {
        Debug.Log(transform.name+ " LEVITATION");
        EnterState(State.Levitating);
    }



    void LaunchObject()
    {
        Instantiate(spoofParticles,transform.position,Quaternion.identity);
        transform.DOKill();
        rigidbody.isKinematic = false;
        ResetVelocity();
        woosh.Play();
        rigidbody.AddTorque(Random.Range(0f, 360f), Random.Range(0f, 360f), Random.Range(0f, 360f));    // Ajout d'une force de rotation
        force = new Vector3(Random.Range(-1f,1f), 0, Random.Range(-1f,1f));
        force = force.normalized * forcePower;
        rigidbody.AddForce(force, ForceMode.VelocityChange);

    }

    public void AbortLaunch()
    {
        transform.DOKill();
        EnterState(State.Ragdoll);
    }

    public void ResetObject()
    {
        transform.DOKill();
        EnterState(State.Start);
    }



    public void ResetVelocity() // Fonction permettant l'arrêt total de la projection
    {
        rigidbody.velocity = Vector3.zero;
        rigidbody.angularVelocity = Vector3.zero;
    }

    public void MouseHover()
    {
        hovered=true;
    }

    public void MouseUnhover()
    {
        hovered=false;
    }

    public void MouseClicDown()
    {
        switch(currentState)
        {
            case State.Start: break;
            case State.Ragdoll:
                EnterState(State.Resetting);
            break;
            case State.Levitating:
                AbortLaunch();
            break;
            case State.Ballistic: break;
            case State.Destroyed: break;
        }
    }


    void OnCollisionEnter(Collision nouvelleCollision)
    {
        float speed = nouvelleCollision.relativeVelocity.magnitude;

        if (speed>10f && chrono>0.2f)
        {
            chrono = 0;
            woosh.Play();
            Instantiate(spoofParticles,nouvelleCollision.contacts[0].point,Quaternion.identity);
        }

        if (speed>breakVelocity) EnterState(State.Destroyed);
    }


    void Update()
    {
        switch(currentState) // UPDATE FUNCTION
        {
            case State.Start:
                if (rigidbody.velocity.sqrMagnitude>0.25f) EnterState(State.Ragdoll);
            break;

            case State.Ragdoll:
            break;

            case State.Levitating:
                chrono+=Time.deltaTime;
                if (chrono>3f) EnterState(State.Ballistic);   
            break;

            case State.Ballistic:
                chrono +=Time.deltaTime;
                if (rigidbody.velocity.sqrMagnitude<0.1f) EnterState(State.Ragdoll);
            break;

            case State.Resetting:
            break;

            case State.Destroyed:
            break;
        }
    }




    void EnterState(State newState)
    {
        Debug.Log(transform.name+" becomes "+newState);
        switch(currentState) // EXIT FUNCTION
        {
            case State.Start:
            break;

            case State.Ragdoll:
            break;

            case State.Levitating:
                if (levitatingParticles) levitatingParticles.Stop();
                baby.EndObjectLevitation();   
            break;

            case State.Ballistic:
                if (trail) trail.enabled = false;
            break;

            case State.Resetting:
            break;

            case State.Destroyed:
            break;
        }

        currentState = newState;
        switch(newState) // START FUNCTION
        {
            case State.Start:
                if (glow) glow.enabled = false;
                transform.position = startPos;
                transform.localEulerAngles = startRot;
                transform.localScale = startScale;
                rigidbody.isKinematic = false;
                ResetVelocity();
            break;

            case State.Ragdoll:
                if (glow) glow.enabled = true;
                rigidbody.isKinematic = false;    
                ResetVelocity();
            break;

            case State.Levitating:
                if (glow) glow.enabled = true;
                if (levitatingParticles) levitatingParticles.Play();
                chrono = 0;               
                rigidbody.isKinematic = true;
                transform.DOMoveY(transform.position.y + 2f,1f).SetEase(Ease.InOutCubic);
                transform.DOLocalRotate(Random.insideUnitSphere * 720f,3f).SetEase(Ease.InSine);
            break;

            case State.Ballistic:
                if (glow) glow.enabled = false;
                chrono = 0.2f;
                if (trail) trail.enabled = true;
                rigidbody.isKinematic = false;                         
                LaunchObject();   
            break;

            case State.Resetting:
                transform.DOKill();
                transform.DOMove(startPos,0.5f).OnComplete(ResetObject);
                transform.DORotate(startRot,.25f);
            break;

            case State.Destroyed:
                if (glow) glow.enabled = false;
                if (breakParticles) Instantiate(breakParticles,transform.position, transform.rotation);
                rigidbody.isKinematic = true;
                gameObject.SetActive(false);
            break;
        }
    } // Fin de EnterState



    bool hovered;
    public bool Hovered
    {
        get {return hovered;}
    }

} // FIN DU SCRIPT
