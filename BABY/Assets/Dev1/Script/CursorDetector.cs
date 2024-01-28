using UnityEngine;


public class CursorDetector : MonoBehaviour
{
    public Camera cam;
    public Ray ray;
    public LayerMask collideWith;

    IInteractable currentinteract;

    void Start(){

    }

    void Update(){
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 500f, collideWith))
        {
            if (Input.GetMouseButtonDown(0)) Debug.Log("clicked on "+hit.transform.name);
            IInteractable interact = hit.transform.GetComponentInParent<IInteractable>();
            if (interact != null)
            {
                if( interact != currentinteract)
                {
                    if (currentinteract!=null) currentinteract.MouseUnhover();
                    currentinteract = interact;
                    interact.MouseHover();
                }
                if (Input.GetMouseButtonDown(0)) interact.MouseClicDown();
            }
            else if (currentinteract != null)
            {
                currentinteract.MouseUnhover();
                currentinteract = null;
            }
        }
        else
        {
            if (currentinteract != null)
            {
                currentinteract.MouseUnhover();
                currentinteract = null;
            }

        }
    }
}
