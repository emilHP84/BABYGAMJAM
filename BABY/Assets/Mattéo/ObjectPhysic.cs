using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class ObjectPhysic : MonoBehaviour, IInteractable
{
    new Rigidbody rigidbody;
    [Range(0f, 100f)] public float forcePower = 50;
    Vector3 force;

    public AudioSource woosh;
    static BabyFSM baby;
    bool aborted = false;
    Vector3 startPos, startRot,startScale;
    [SerializeField]GameObject spoofParticles;

    void Awake()
    {
        startPos = transform.position;
        startRot = transform.localEulerAngles;
        startScale = transform.localScale;
        baby = GameObject.FindObjectOfType<BabyFSM>();
        rigidbody = GetComponent<Rigidbody>();                                                          
    }

    public void StartProjection()
    {
        Debug.Log(transform.name);
        StartCoroutine(PhysicsProjection());
    }

    public IEnumerator PhysicsProjection()
    {
        aborted = false;
        rigidbody.isKinematic = true;
        transform.DOMoveY(transform.position.y + 2f,1f).SetEase(Ease.InOutCubic);
        transform.DOLocalRotate(Random.insideUnitSphere * 720f,3f).SetEase(Ease.InSine);
        //rigidbody.AddForce(0, 20f, 0, ForceMode.VelocityChange); 
        float chrono = 3f;
        while (chrono>0 && aborted==false)
        {
            chrono -=Time.deltaTime;
            yield return null;
        }
        rigidbody.isKinematic = false;                         
        if (aborted==false) LaunchObject();   
        baby.EndObjectLevitation();                                                                                                            // Fin de la gestion de la physique
    }

    void LaunchObject()
    {
        Instantiate(spoofParticles,transform.position,Quaternion.identity);
        transform.DOKill();
        rigidbody.isKinematic = false;
        rigidbody.velocity = Vector3.zero;
        rigidbody.angularVelocity= Vector3.zero;
        woosh.Play();
        rigidbody.AddTorque(Random.Range(0f, 360f), Random.Range(0f, 360f), Random.Range(0f, 360f));    // Ajout d'une force de rotation
        force = new Vector3(Random.Range(-1f,1f), 0, Random.Range(-1f,1f));
        force = force.normalized * forcePower;
        rigidbody.AddForce(force, ForceMode.VelocityChange);

    }

    public void AbortLaunch()
    {
        transform.DOKill();
        aborted = true;
    }

    public void ResetObject()
    {
        transform.position = startPos;
        transform.localEulerAngles = startRot;
        transform.localScale = startScale;
        transform.DOKill();
        rigidbody.isKinematic = false;
        rigidbody.velocity = Vector3.zero;
        rigidbody.angularVelocity = Vector3.zero;
    }



    public void StopPhysics() // Fonction permettant l'arrêt total de la projection
    {
        rigidbody.velocity = Vector3.zero;
        rigidbody.angularVelocity = Vector3.zero;
    }

    public void MouseHover()
    {

    }

    public void MouseUnhover()
    {

    }

    public void MouseClicDown()
    {
        AbortLaunch();
    }

    void OnCollisionEnter(Collision nouvelleCollision)
    {
        if (nouvelleCollision.relativeVelocity.magnitude>10f)
        {
            woosh.Play();
            Instantiate(spoofParticles,nouvelleCollision.contacts[0].point,Quaternion.identity);
        }
    }
} // FIN DU SCRIPT
