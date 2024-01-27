using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class CursorDetector : MonoBehaviour
{
    public Camera cam;
    public Ray ray;

    IInteractable currentinteract;

    void Start(){

    }

    void Update(){
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            IInteractable interact = GetComponentInParent<IInteractable>();
            if (interact != null)
            {
                if( interact != currentinteract)
                {
                    currentinteract = interact;
                    interact.Glowing();
                }
            }
            else if (currentinteract != null)
            {
                currentinteract.UnGlowing();
                currentinteract = null;
            }
        }
        else
        {
            if (currentinteract != null)
            {
                currentinteract.UnGlowing();
                currentinteract = null;
            }

        }
    }
}
