using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetObjectPositionScript : MonoBehaviour, IInteractable
{
    private Vector3 originalPosition;
    private Quaternion originalRotation;
    public int clickBuffer = 5;
    int currentBuffer;

    void Start()
    {
        this.originalPosition = this.transform.position;
	    this.originalRotation = this.transform.rotation;
        ResetPosition();
    }

    public void MouseHover()
    {
        hovered = true;
    }

    public void MouseUnhover()
    {
        hovered=false;
    }

    public void MouseClicDown()
    {
        Debug.Log(transform.name);
        clickBuffer--;
        if (clickBuffer<1) ResetPosition();
    }




    void ResetPosition() {
        this.transform.position = this.originalPosition;
	    this.transform.rotation = this.originalRotation;
        currentBuffer = clickBuffer;
        Debug.Log("Reset de la position de " + gameObject.name + ".");
        GetComponent<ObjectPhysic>().ResetVelocity();
    }


    bool hovered;
    public bool Hovered
    {
        get {return hovered;}
    }

} // FIN DU SCRIPT