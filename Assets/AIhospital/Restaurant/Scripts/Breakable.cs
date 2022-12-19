using UnityEngine;
using System.Collections;


[RequireComponent(typeof(AudioSource))]
public class Breakable : MonoBehaviour {

	public GameObject shards;
	public int resistance;

	AudioSource audio;

	public AudioClip collide;


	void Start() {
		audio = GetComponent<AudioSource> ();

	}

	void Update() {

	}

	void OnCollisionEnter(Collision col){
		if (col.relativeVelocity.magnitude > resistance) {
			
			Instantiate (shards, transform.position, transform.rotation);
			Destroy (gameObject);

		} else {
			audio.PlayOneShot (collide, 0.1F*col.relativeVelocity.magnitude);
		}

	}

}