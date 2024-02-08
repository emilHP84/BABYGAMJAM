using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonScript : MonoBehaviour
{
    [SerializeField]AudioClip buttonClick;

    public void Clic()
    {
        if (buttonClick) AudioSource.PlayClipAtPoint(buttonClick,Camera.main.transform.position);
    }
}
