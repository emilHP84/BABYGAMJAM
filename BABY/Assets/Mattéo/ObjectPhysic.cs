using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPhysic : MonoBehaviour, IInteractable
{
    new Rigidbody rigidbody;
    [Range(0f, 100f)] public float forcePower = 50;
    Vector3 force;

    public AudioSource woosh;
    static BabyFSM baby;
    bool aborted = false;

    void Awake()
    {
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
        rigidbody.AddForce(0, 20f, 0, ForceMode.VelocityChange); 
        float chrono = 1f;
        while (chrono>0 && aborted==false)
        {
            chrono -=Time.deltaTime;
            yield return null;
        }                                 
        if (aborted==false) LaunchObject();   
        baby.EndObjectLevitation();                                                                                                            // Fin de la gestion de la physique
    }

    void LaunchObject()
    {
        woosh.Play();
        rigidbody.AddTorque(Random.Range(0f, 360f), Random.Range(0f, 360f), Random.Range(0f, 360f));    // Ajout d'une force de rotation
        force = new Vector3(Random.Range(-50f, 50f), 0, Random.Range(-50f, 50f));
        force = force.normalized * forcePower;
        rigidbody.AddForce(force, ForceMode.VelocityChange);

    }

    public void AbortLaunch()
    {
        aborted = true;
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
} // FIN DU SCRIPT
