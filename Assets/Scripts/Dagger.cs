using System;
using UnityEngine;

public class Dagger : MonoBehaviour
{
    public int index;
    public AudioClip finishedAudioClip;
    public AudioClip collisionAudioClip;
    private AudioSource audioSource;
    private ParticleSystem particleSystem;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        particleSystem = GetComponentInChildren<ParticleSystem>();
    }

    public void PlayFinishedSound()
    {
        particleSystem.Play();
        audioSource.PlayOneShot(finishedAudioClip);
    }
    
    private void OnCollisionEnter(Collision other)
    {
        if (!audioSource.enabled)
            return;
        if(!audioSource.isPlaying)
            audioSource.PlayOneShot(collisionAudioClip);
    }
}
