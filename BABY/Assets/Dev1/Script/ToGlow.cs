using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToGlow : MonoBehaviour, IInteractable
{
    public void Glowing()
    {
        Material glowMaterial = gameObject.GetComponent<MeshRenderer>().materials[1];
    }
}
