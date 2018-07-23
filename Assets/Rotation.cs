using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotation : MonoBehaviour {

    [SerializeField] float rotationSpeed = 50f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        transform.Rotate(Vector3.up * Time.deltaTime * rotationSpeed, Space.World);
        transform.Rotate(Vector3.left * Time.deltaTime * rotationSpeed, Space.World);

    }
}
