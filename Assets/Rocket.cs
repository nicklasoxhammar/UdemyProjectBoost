
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour {

    [SerializeField] float rcsThrust = 100f;
    [SerializeField] float mainThrust = 20f;

    [SerializeField] AudioClip mainEngineSound;
    [SerializeField] AudioClip deathSound;
    [SerializeField] AudioClip levelCompleteSound;

    [SerializeField] ParticleSystem mainEngineParticles;
    [SerializeField] ParticleSystem deathParticles;
    [SerializeField] ParticleSystem levelCompleteParticles;

    Rigidbody rigidBody;
    AudioSource audioSource;
    AudioSource rocketAudio;
    float rocketSoundVolume;

    enum State { Alive, Dying, Transcending }
    State state = State.Alive;

	// Use this for initialization
	void Start () {

        AudioSource[] audioSources = GetComponents<AudioSource>();
        audioSource = audioSources[0];
        rocketAudio = audioSources[1];

        rigidBody = GetComponent<Rigidbody>();
       // audioSource = GetComponent<AudioSource>();
        rocketSoundVolume = rocketAudio.volume;
        

		
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
                StartCoroutine(VolumeFade(rocketAudio, 0f, 0.5f));
                audioSource.PlayOneShot(levelCompleteSound);
                levelCompleteParticles.Play();
                DestroyShip();
                Invoke("LoadNextScene", 2.3f);
                break;

            default:
                state = State.Dying;
                StartCoroutine(VolumeFade(rocketAudio, 0f, 0.5f));
                audioSource.PlayOneShot(deathSound);
                deathParticles.Play();
                DestroyShip();
                Invoke("ReloadLevel", 2.3f);
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

            if (!rocketAudio.isPlaying) {
               rocketAudio.PlayOneShot(mainEngineSound);
            }

            mainEngineParticles.Play();
        }
       
        if (Input.GetKeyUp(KeyCode.Space)) {
            StartCoroutine(VolumeFade(rocketAudio, 0f, 0.5f));
            mainEngineParticles.Stop();
        }
    }

    void DestroyShip() {
        this.gameObject.transform.GetChild(0).gameObject.SetActive(false);
        mainEngineParticles.Stop();  
    }

    void ReloadLevel() {
    
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

    }

    IEnumerator VolumeFade(AudioSource _AudioSource, float _EndVolume, float _FadeLength) {

        float _StartVolume = _AudioSource.volume;

        float _StartTime = Time.time;

        while (Time.time < _StartTime + _FadeLength) {

            _AudioSource.volume = _StartVolume + ((_EndVolume - _StartVolume) * ((Time.time - _StartTime) / _FadeLength));

            yield return null;

        }

        if (_EndVolume == 0) { _AudioSource.Stop(); }

        rocketAudio.volume = rocketSoundVolume;

    }

}
