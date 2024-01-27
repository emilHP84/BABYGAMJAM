using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetObjectPositionScript : MonoBehaviour
{
    private Vector3 originalPosition;
    private Quaternion originalRotation;

    void Start() {
        this.originalPosition = this.transform.position;
	    this.originalRotation = this.transform.rotation;
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Space) /* Remplacer par la condition de clic sur l'objet */)
        {
            ResetPosition();
        }
    }

    void ResetPosition() {
        this.transform.position = this.originalPosition;
	    this.transform.rotation = this.originalRotation;
        Debug.Log("Reset de la position de " + gameObject.name + ".");
        GetComponent<ObjectPhysic>().StopPhysics();
    }


} // FIN DU SCRIPT