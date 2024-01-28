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
            IInteractable interact = hit.transform.GetComponentInParent<IInteractable>();
            if (interact != null)
            {
                if( interact != currentinteract)
                {
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
