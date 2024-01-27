using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPhysic : MonoBehaviour
{
    Rigidbody rigidbody;

    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        StartCoroutine(physicsProjection());
    }

    public IEnumerator physicsProjection() {
        rigidbody.AddForce(0, 100, 0);
        yield return new WaitForSeconds(1);
        rigidbody.AddTorque(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360));
        rigidbody.AddForce(Random.Range(0, 500), 0, Random.Range(0, 500));
        yield return null;
    }
}
