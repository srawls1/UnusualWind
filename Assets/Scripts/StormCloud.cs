using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StormCloud : MonoBehaviour
{
    [SerializeField] private float seedDropDuration;

    private Animator animator;
    private GameObject lightning;
    private AudioSource lightningAudio;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        lightning = transform.GetChild(0).gameObject;
        lightningAudio = GetComponent<AudioSource>();
    }

    //Whenever player collides with cloud, lightning strikes
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            //flash lightning on and off twice
            animator.SetTrigger("Strike");
            //If lightning audio is not playing, play it
            if (!lightningAudio.isPlaying)
            {
                //Set lightning audio to a random pitch
                lightningAudio.pitch = Random.Range(.8f, 1.2f);
                lightningAudio.Play();
            }

            //Temporarily force player to drop
            SeedInWind seed = collision.GetComponent<SeedInWind>();
            seed.Drop(seedDropDuration);
        }
    }
}