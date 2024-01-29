using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ToGlow : MonoBehaviour, IInteractable
{
    private MeshRenderer[] rends;
    MaterialSwitch[] mats;

    void Awake() {
        rends = gameObject.GetComponentsInChildren<MeshRenderer>();
        if (rends.Length<1) Destroy(this);
        mats = new MaterialSwitch[rends.Length];
        Material glowMat = Resources.Load<Material>("glow");
        for (int i=0; i<rends.Length;i++)
        {
            mats[i] = new MaterialSwitch();
            mats[i].matwithglow = new Material[2];
            mats[i].matnormal = new Material[1];
            mats[i].matwithglow[1] = glowMat;
            mats[i].matwithglow[0] = mats[i].matnormal[0] = rends[i].materials[0];
        }
    }

    public void MouseHover(){
        for (int i=0; i<rends.Length;i++)
        rends[i].materials = mats[i].matwithglow;
        Debug.Log("Glowing");
    }

    public void MouseUnhover(){
        for (int i=0; i<rends.Length;i++)
        rends[i].materials = mats[i].matnormal;
        Debug.Log("Unglowing");
    }

    public void MouseClicDown()
    {

    }
}

public class MaterialSwitch
{
    public MaterialSwitch()//Material matNormal, Material matWithGlow)
    {
        matwithglow = new Material[2];
        matnormal = new Material[1];
        //matnormal[0] = matwithglow[0] = matNormal;
        //matwithglow[1] = matWithGlow;
    }

    public Material[] matwithglow, matnormal;
}
