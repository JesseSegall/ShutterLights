using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeRoomZombieAudio : MonoBehaviour
{
    public AudioClip creepyZombieSound; // Assign your MP3 clip here

    private AudioSource _audioSource;
    private bool _hasPlayed = false;

    private void Start()
    {

        _audioSource = GetComponent<AudioSource>();


    }


    private void OnTriggerEnter(Collider other)
    {
        if (!_hasPlayed && other.CompareTag("Player"))
        {
            _audioSource.PlayOneShot(creepyZombieSound);
        }
    }
}
