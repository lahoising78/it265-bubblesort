using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseClickRayCaster : MonoBehaviour {

    public LayerMask clickMask;
    public Transform cubeTransform;

    Vector3 clickPosition;
    Ray ray;
    RaycastHit hit; //dont have to assign this as the raycast will assign this procedurally

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            clickPosition = -Vector3.one;
            ray = Camera.main.ScreenPointToRay(Input.mousePosition); //dont need a z distance
            if (Physics.Raycast(ray, out hit, 100f, clickMask)) //need to add max distance, and layerMask
            {
                clickPosition = hit.point;
                cubeTransform.position = clickPosition;
            }
        }
    }
}
