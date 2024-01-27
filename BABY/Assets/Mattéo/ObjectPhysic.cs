using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPhysic : MonoBehaviour
{
    new Rigidbody rigidbody;
    [Range(0f, 100f)] public float forcePower = 50;

    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();                                                          
        StartCoroutine(physicsProjection());
    }

    public IEnumerator physicsProjection() {
        rigidbody.AddForce(0, 100, 0);                                                                  // Montée de l'objet
        yield return new WaitForSeconds(1);                                                             // On attend 1 sec
        rigidbody.AddTorque(Random.Range(0f, 360f), Random.Range(0f, 360f), Random.Range(0f, 360f));    // Ajout d'une force de rotation
        Vector3 force = new Vector3(Random.Range(-50f, 50f), 0, Random.Range(-50f, 50f));
        force = force.normalized * forcePower;
        rigidbody.AddForce(force, ForceMode.VelocityChange);                                            // Projection de l'objet
        yield return null;                                                                              // Fin de la gestion de la physique
    }
}
