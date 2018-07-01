using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class Oscillator : MonoBehaviour {

    [SerializeField] Vector3 movementVector;

    // todo remove from inspector later
    [Range(0,1)][SerializeField]
    float movementFactor; // 0 for not moves, 1 for fully moved.

    Vector3 startingPos;

	// Use this for initialization
	void Start () {
        startingPos = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
        //set movement factor

        Vector3 offset = movementFactor * movementVector;
        transform.position = startingPos + offset;

	}
}
