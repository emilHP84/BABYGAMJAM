using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPhysic : MonoBehaviour
{
    new Rigidbody rigidbody;
    [Range(0f, 100f)] public float forcePower = 50;
    Vector3 force;

    public AudioSource woosh;

    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();                                                          
        
    }

    public void StartProjection()
    {
        Debug.Log(transform.name);
        StartCoroutine(physicsProjection());
    }

    public IEnumerator physicsProjection() {
        rigidbody.AddForce(0, 100, 0);                                                                  // Montée de l'objet
        yield return new WaitForSeconds(1);                                                             // On attend 1 sec
        woosh.Play();
        rigidbody.AddTorque(Random.Range(0f, 360f), Random.Range(0f, 360f), Random.Range(0f, 360f));    // Ajout d'une force de rotation
        force = new Vector3(Random.Range(-50f, 50f), 0, Random.Range(-50f, 50f));
        force = force.normalized * forcePower;
        rigidbody.AddForce(force, ForceMode.VelocityChange);                                            // Projection de l'objet
        yield return null;                                                                              // Fin de la gestion de la physique
    }

    public void StopPhysics() // Fonction permettant l'arrêt total de la projection
    {
        rigidbody.velocity = Vector3.zero;
        rigidbody.angularVelocity = Vector3.zero;
    }
} // FIN DU SCRIPT
