using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeSounds : MonoBehaviour {

    AudioSource audioSource;
    AudioClip impactClip;

	// Use this for initialization
	void Start () {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.spatialize = true;
        audioSource.spatialBlend = 1.0f;
        audioSource.dopplerLevel = 0.0f;
        audioSource.rolloffMode = AudioRolloffMode.Logarithmic;
        audioSource.maxDistance = 20f;

        impactClip = Resources.Load<AudioClip>("Cardboard_audio_4");
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnCollisionEnter(Collision collision)
    {
        audioSource.clip = impactClip;
        audioSource.Play();
    }
}
