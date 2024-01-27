using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetObjectPositionScript : MonoBehaviour
{
    private Vector3 originalPosition;
    private Quaternion originalRotation;
    public CursorDetector cursorDetector;
    public int clickBuffer = 5;

    void Start() {
        this.originalPosition = this.transform.position;
	    this.originalRotation = this.transform.rotation;
    }

    void OnMouseOver()
    {
        if(Input.GetMouseButtonDown(0))
        {
            clickBuffer--;
            if(clickBuffer <= 0)
            {
                ResetPosition();
            }
        }
    }

    void ResetPosition() {
        this.transform.position = this.originalPosition;
	    this.transform.rotation = this.originalRotation;
        Debug.Log("Reset de la position de " + gameObject.name + ".");
        GetComponent<ObjectPhysic>().StopPhysics();
    }


} // FIN DU SCRIPT