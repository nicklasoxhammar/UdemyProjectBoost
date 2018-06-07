using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour {

    [SerializeField]
    float rcsThrust = 100f;

    [SerializeField]
    float mainThrust = 20f;

    Rigidbody rigidBody;
    AudioSource audioSource;
    float rocketSoundVolume;

	// Use this for initialization
	void Start () {

        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
        rocketSoundVolume = audioSource.volume;
        

		
	}
	
	// Update is called once per frame
	void Update () {
        Rotate();
        Thrust();

    }

    private void OnCollisionEnter(Collision collision) {

        if(collision.gameObject.tag != "Friendly") {
            DestroyShip();
        }
    }


    private void Rotate() {

        rigidBody.freezeRotation = true;

        float rotationThisFrame = rcsThrust * Time.deltaTime;

        if (Input.GetKey(KeyCode.A)) {
            transform.Rotate(Vector3.forward * rotationThisFrame);
        }
        else if (Input.GetKey(KeyCode.D)) {
            transform.Rotate(-Vector3.forward * rotationThisFrame);
        }

        rigidBody.freezeRotation = false;

    }

    private void Thrust() {

        if (Input.GetKey(KeyCode.Space)) {

            rigidBody.AddRelativeForce(Vector3.up * mainThrust);

            if (!audioSource.isPlaying) {
                audioSource.volume = rocketSoundVolume;
                audioSource.Play();
            }
        }
       
        if (Input.GetKeyUp(KeyCode.Space)) {
            StartCoroutine(VolumeFade(audioSource, 0f, 0.5f));
        }
    }

    void DestroyShip() {
        Destroy(this.gameObject);

    }

    IEnumerator VolumeFade(AudioSource _AudioSource, float _EndVolume, float _FadeLength) {

        float _StartVolume = _AudioSource.volume;

        float _StartTime = Time.time;

        while (Time.time < _StartTime + _FadeLength) {

            _AudioSource.volume = _StartVolume + ((_EndVolume - _StartVolume) * ((Time.time - _StartTime) / _FadeLength));

            yield return null;

        }

        if (_EndVolume == 0) { _AudioSource.Stop(); }

    }

}
