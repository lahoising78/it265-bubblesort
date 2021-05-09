using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingHandle : MonoBehaviour {


    public Transform handleSwapCenter;
    float distanceFromCenter;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void LateUpdate()
    {
        transform.localRotation = Quaternion.identity;
        distanceFromCenter = Vector3.Distance(handleSwapCenter.position, transform.position);
        Debug.Log(distanceFromCenter);
        //find vector from center to handle
        //change magnitude of vector to 0.0242 to keep handle in same relative distance from center
        //figure out rotation of button based off the local x and z of handle
        //figure out if handle is moving through quadrants to perform a full turn which means execute a swap

    }
}
