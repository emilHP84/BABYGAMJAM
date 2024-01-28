using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ToGlow : MonoBehaviour, IInteractable
{
    private Renderer rend;

    Material[] matwithglow, matnormal;
    void Awake() {
        rend = gameObject.GetComponentInChildren<Renderer>();
        matwithglow = new Material[2];
        matwithglow[1] = Resources.Load<Material>("glow");
        matnormal = new Material[1];
        matwithglow[0] = matnormal[0] = rend.materials[0];

    }

    public void MouseHover(){
        //gameObject.GetComponent<Renderer>().materials = mat;
        rend.materials = matwithglow;
        Debug.Log("Glowing");
    }

    public void MouseUnhover(){
        rend.materials = matnormal;
        Debug.Log("Unglowing");
    }

    public void MouseClicDown()
    {

    }
}
