using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Environment : MonoBehaviour {

    [SerializeField] float skyboxRotationSpeed = 0.3f;

	// Use this for initialization
	void Start () {

        DontDestroyOnLoad(this.gameObject);
		
	}
	
	// Update is called once per frame
	void Update () {

        RenderSettings.skybox.SetFloat("_Rotation", Time.time * skyboxRotationSpeed);

    }
}
