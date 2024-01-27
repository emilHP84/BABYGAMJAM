using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ToGlow : MonoBehaviour, IInteractable
{
    private Renderer rend;

    Material[] matwithglow, matnormal;
    void Awake() {
        rend = gameObject.GetComponent<Renderer>();
        matwithglow = new Material[2];
        matwithglow[1] = Resources.Load<Material>("glow");
        matnormal = new Material[1];
        matwithglow[0] = matnormal[0] = rend.materials[0];

    }

    private void Start(){
        
        
       
    }

    public void Glowing(){
        //gameObject.GetComponent<Renderer>().materials = mat;
        gameObject.GetComponent<Renderer>().materials = matwithglow;
        Debug.Log("Glowing");
    }

    public void UnGlowing(){
        gameObject.GetComponent<Renderer>().materials = matnormal;
        Debug.Log("Unglowing");
    }
}
