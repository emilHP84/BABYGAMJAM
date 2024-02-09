using System.Linq;
using UnityEngine;
using UnityEngine.AI;


public class CursorDetector : MonoBehaviour
{
    public Camera cam;
    public Ray ray;
    public LayerMask collideWith;

    IInteractable[] currentinteracts;

 
    void Update(){
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 500f, collideWith))
        {
            if (Input.GetMouseButtonDown(0)) Debug.Log("clicked on "+hit.transform.name);
            IInteractable[] interact = hit.transform.GetComponentsInParent<IInteractable>();
            if (interact.Length>0)
            {
                for (int i=0; i<interact.Length; i++) // Si un objet était sous la souris mais ne l'est plus, on le déselectionne
                {
                    if (interact[i].Hovered==false) interact[i].MouseHover();
                    if (Input.GetMouseButtonDown(0)) interact[i].MouseClicDown(); // Événement si on clic sur l'objet
                }
            }
            if (currentinteracts!=null && currentinteracts.Length>0)
            {
                for (int i=0; i<currentinteracts.Length;i++)
                {
                    if (!interact.Contains(currentinteracts[i])) currentinteracts[i].MouseUnhover();
                }
            }
            currentinteracts = interact;
        } // Sinon, le raycast de la souris n'a rien touché
        else if (currentinteracts!=null)
        {
            for (int i=0; i<currentinteracts.Length;i++) if (currentinteracts[i].Hovered) currentinteracts[i].MouseUnhover();
            currentinteracts = null;     
        }

    } //Fin de Update()



} // FIN DU SCRIPT
