using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OceanAudioCheck : MonoBehaviour
{
    [SerializeField] private AudioSource splashSound;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        PlaySound();
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        PlaySound();
    }

    private void PlaySound(){
        if (!splashSound.isPlaying)
        {
            //randomize pitch
            splashSound.pitch = Random.Range(0.8f, 1.2f);
            splashSound.Play();
        }
    }
}