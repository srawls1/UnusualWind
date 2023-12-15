using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OceanAudioCheck : MonoBehaviour
{
    [SerializeField] private AudioSource splashSound;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!splashSound.isPlaying)
        {
            splashSound.Play();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!splashSound.isPlaying)
        {
            splashSound.Play();
        }
    }
}