
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour {

    [SerializeField] float rcsThrust = 100f;
    [SerializeField] float mainThrust = 20f;
    [SerializeField] AudioClip mainEngineSound;
    [SerializeField] AudioClip deathSound;
    [SerializeField] AudioClip levelCompleteSound;

    Rigidbody rigidBody;
    AudioSource audioSource;
    float rocketSoundVolume;

    enum State { Alive, Dying, Transcending }
    State state = State.Alive;

	// Use this for initialization
	void Start () {

        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
        rocketSoundVolume = audioSource.volume;
        

		
	}

    // Update is called once per frame
    void Update() {

        if (state == State.Alive) { 
        Rotate();
        Thrust();
    }

    }

    private void OnCollisionEnter(Collision collision) {

        if (state != State.Alive) { return; } //ignore collisions when not alive

        switch (collision.gameObject.tag) {

            case "Friendly":
                break;

            case "Finish":
                state = State.Transcending;
                StartCoroutine(VolumeFade(audioSource, 0f, 0.5f));
                audioSource.PlayOneShot(levelCompleteSound);
                Invoke("LoadNextScene", 1f);
                break;

            default:
                state = State.Dying;
                StartCoroutine(VolumeFade(audioSource, 0f, 0.5f));
                audioSource.PlayOneShot(deathSound);
                Invoke("DestroyShip", 1f);
                break;

        }

    }

    private void LoadNextScene() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
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
                audioSource.PlayOneShot(mainEngineSound);
            }
        }
       
        if (Input.GetKeyUp(KeyCode.Space)) {
            StartCoroutine(VolumeFade(audioSource, 0f, 0.5f));
        }
    }

    void DestroyShip() {

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        //Destroy(this.gameObject);

    }

    IEnumerator VolumeFade(AudioSource _AudioSource, float _EndVolume, float _FadeLength) {

        float _StartVolume = _AudioSource.volume;

        float _StartTime = Time.time;

        while (Time.time < _StartTime + _FadeLength) {

            _AudioSource.volume = _StartVolume + ((_EndVolume - _StartVolume) * ((Time.time - _StartTime) / _FadeLength));

            yield return null;

        }

        if (_EndVolume == 0) { _AudioSource.Stop(); }

        audioSource.volume = rocketSoundVolume;

    }

}
