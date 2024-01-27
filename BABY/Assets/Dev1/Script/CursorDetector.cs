using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class CursorDetector : MonoBehaviour
{
    private Camera cam;
    Vector3 pos = new Vector3(0, 0, 100);

    void Awake() { cam = GetComponent<Camera>(); }

    void Start(){
    }

    void Update(){
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity)){
            // appeler glowing
        }
    }

    
}
